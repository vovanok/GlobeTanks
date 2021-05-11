using UnityEngine;
using UniRx;
using Zenject;

namespace GlobeTanks.Objects
{
	public class ProjectileInfluence : MonoBehaviour
	{
		public float Radius { get; private set; }

		private float curRadius;
		private int stage;
		private float timeExpansion = 1f;
		private float timeStay = 1f;
		private float curStageTime;

		public ReactiveCommand<ProjectileInfluence> OnPeak { get; } = new ReactiveCommand<ProjectileInfluence>();
		public ReactiveCommand<ProjectileInfluence> OnEnd { get; } = new ReactiveCommand<ProjectileInfluence>();

		[Inject]
		public void Construct(Vector3 position, float radius)
		{
			transform.position = position;
			Radius = radius;
		}

		private void Update()
		{
			curStageTime += Time.deltaTime;

			if (stage == 0)
			{
				curRadius = Mathf.Lerp(0f, Radius, curStageTime / timeExpansion);
				if (curRadius >= Radius)
				{
					stage++;
					curStageTime = 0;

					OnPeak.Execute(this);
				}
			}

			if (stage == 1)
			{
				if (curStageTime >= timeStay)
				{
					stage++;
					curStageTime = 0;
				}
			}

			if (stage == 2)
			{
				curRadius = Mathf.Lerp(Radius, 0f, curStageTime / timeExpansion);
				if (curRadius <= 0f)
				{
					stage++;
					curStageTime = 0;

					OnEnd.Execute(this);
				}
			}

			if (stage == 3)
				Destroy(gameObject);

			transform.localScale = new Vector3(curRadius, curRadius, curRadius);
		}
	}
}
