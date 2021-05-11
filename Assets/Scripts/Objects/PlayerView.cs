using UnityEngine;
using UniRx;
using Zenject;
using GlobeTanks.Args;
using GlobeTanks.ViewModels;

namespace GlobeTanks.Objects
{
	public class PlayerView : MonoBehaviour
	{
		private IFactory<Vector3, Vector3, float, ProjectileView> projectileViewFactory;

		[SerializeField] private Renderer[] renderers;
		[SerializeField] private Transform shootSrcPivot;
		[SerializeField] private ObjectOnEarth objectOnEarth;

		public PlayerModel Model { get; private set; }

		public ReactiveCommand<ShootEventArgs> OnShoot { get; } = new ReactiveCommand<ShootEventArgs>();
		public ReactiveCommand<PlayerView> OnEndStep { get; } = new ReactiveCommand<PlayerView>();

		private bool isShootLocked;

		[Inject]
		public void Construct(IFactory<Vector3, Vector3, float, ProjectileView> projectileViewFactory, 
			ShootDirectionView shootDirectionView, PlayerModel model)
		{
			this.projectileViewFactory = projectileViewFactory;
			Model = model;
			gameObject.name = name;

			transform.position = model.InitialPosition;
			transform.rotation = Quaternion.Euler(model.InitialRotation);

			foreach (Renderer curRenderer in renderers)
				curRenderer.material.color = model.Color.Value;

            model.Fire
                .Subscribe(_ => Shoot())
                .AddTo(this);

			model.IsCurrent
				.Subscribe(value =>
				{
					if (value)
					{
						shootDirectionView.Refresh(shootSrcPivot.position, GetShootDirection());
						isShootLocked = false;
					}
				})
				.AddTo(this);

			model.ShootDirectionAngle
				.Subscribe(angle =>
				{
					if (model.IsCurrent.Value)
						shootDirectionView.Refresh(shootSrcPivot.position, GetShootDirection());
				})
				.AddTo(this);

			model.Hp
				.Subscribe(value =>
				{
					if (value <= 0)
						gameObject.SetActive(false);
				}).AddTo(this);
        }

		private void Shoot()
		{
			if (isShootLocked)
				return;

			isShootLocked = true;

			Vector3 shootDir = GetShootDirection();
			ProjectileView projectile = projectileViewFactory.Create(shootSrcPivot.position, shootDir, 2f);

			projectile.OnInfluenceEnd
				.Subscribe(_ => OnEndStep.Execute(this))
				.AddTo(this);

			OnShoot.Execute(new ShootEventArgs(this, projectile));
		}

		private Vector3 GetShootDirection()
		{
			return Quaternion.Euler(Model.ShootDirectionAngle.Value.x, 0, Model.ShootDirectionAngle.Value.y) * transform.up;
		}
	}
}