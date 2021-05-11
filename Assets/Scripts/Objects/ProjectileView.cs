using UnityEngine;
using UniRx;
using Zenject;
using GlobeTanks.Args;
using GlobeTanks.Common;

namespace GlobeTanks.Objects
{
	public class ProjectileView : MonoBehaviour
	{
		[SerializeField] private Rigidbody rigidbody;

		private ProjectileTracerView projectileTracer;
		private SphereTerrain terrain;
		private IFactory<Vector3, float, ProjectileInfluence> projectileInfluenceFactory;
		private Vector3 sourcePosition;
		private Vector3 direction;
		private float force;
		private Vector3 previousPosition;

		public ReactiveCommand<InfluenceEventArgs> OnInfluencePeak { get; } = new ReactiveCommand<InfluenceEventArgs>();
		public ReactiveCommand<InfluenceEventArgs> OnInfluenceEnd { get; } = new ReactiveCommand<InfluenceEventArgs>();

		[Inject]
		public void Construct(ProjectileTracerView projectileTracer, SphereTerrain terrain, IFactory<Vector3, float, ProjectileInfluence> projectileInfluenceFactory,
			Vector3 sourcePosition, Vector3 direction, float force)
		{
			this.projectileTracer = projectileTracer;
			this.terrain = terrain;
			this.projectileInfluenceFactory = projectileInfluenceFactory;
			this.sourcePosition = sourcePosition;
			this.direction = direction;
			this.force = force;

			transform.position = sourcePosition;
			previousPosition = transform.position;

			projectileTracer.Clear();
			projectileTracer.AddPoint(sourcePosition);
		}

        private void Start()
        {
			rigidbody.AddForce(direction.normalized * force, ForceMode.Impulse);
		}

		private void Update()
		{
			projectileTracer.AddPoint(transform.position);

			if (previousPosition == transform.position)
				return;

			Vector3 hitPosition;
			if (terrain.Linecast(previousPosition, transform.position, out hitPosition))
			{
				gameObject.SetActive(false);

				ProjectileInfluence influence = projectileInfluenceFactory.Create(transform.position, Consts.BOOM_RADIUS);
				influence.transform.localScale = Vector3.zero;

				influence.OnPeak
					.Subscribe(OnInnerInfluencePeak)
					.AddTo(influence);

				influence.OnEnd
					.Subscribe(OnInnerInfluenceEnd)
					.AddTo(influence);
			}

			previousPosition = transform.position;
		}

		private void OnInnerInfluencePeak(ProjectileInfluence projectileInfluence)
		{
			projectileInfluence.OnPeak.Dispose();

			OnInfluencePeak.Execute(new InfluenceEventArgs(this, projectileInfluence));
		}

		private void OnInnerInfluenceEnd(ProjectileInfluence projectileInfluence)
		{
			projectileInfluence.OnEnd.Dispose();

			Destroy(gameObject);

			OnInfluenceEnd.Execute(new InfluenceEventArgs(this, projectileInfluence));
		}
	}
}
