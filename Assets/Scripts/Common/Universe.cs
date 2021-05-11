using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using GlobeTanks.Args;
using GlobeTanks.Objects;
using GlobeTanks.ViewModels;

namespace GlobeTanks.Common
{
	public class Universe : IInitializable, IDisposable
	{
		private readonly SphereTerrain terrain;
		private readonly ProjectileTracerView projectileTracer;
		private readonly IFactory<PlayerModel, PlayerView> playerViewFactory;
		private readonly GameModel gameModel;

		private List<PlayerView> playersViews = new List<PlayerView>();
		private CompositeDisposable disposables = new CompositeDisposable();

		public Universe(SphereTerrain terrain, ProjectileTracerView projectileTracer,
			IFactory<PlayerModel, PlayerView> playerViewFactory, GameModel gameModel)
		{
			this.terrain = terrain;
			this.projectileTracer = projectileTracer;
			this.playerViewFactory = playerViewFactory;
			this.gameModel = gameModel;
		}

		public void Initialize()
		{
			projectileTracer.Clear();

			foreach (PlayerModel player in gameModel.Players)
				AddPlayer(player);

			gameModel.Players
				.ObserveAdd()
				.Subscribe(args => AddPlayer(args.Value))
				.AddTo(disposables);

			gameModel.Players
				.ObserveRemove()
				.Subscribe(args => RemovePlayer(args.Value))
				.AddTo(disposables);
		}

		public void Dispose()
		{
			disposables.Clear();
		}

		public void StartGame()
		{
            PlayerModel startPlayer = gameModel.Players[UnityEngine.Random.Range(0, gameModel.Players.Count)];
			SetCurrentPlayer(startPlayer);
		}

		private void AddPlayer(PlayerModel model)
		{
			PlayerView newPlayer = playerViewFactory.Create(model);

			newPlayer.OnShoot
				.Subscribe(args =>
				{
					args.Projectile.OnInfluencePeak
						.Subscribe(OnPlayerProjectileInfluencePeak)
						.AddTo(disposables);
				}).AddTo(newPlayer);

			newPlayer.OnEndStep
				.Subscribe(_ => NextPlayer())
				.AddTo(newPlayer);

			playersViews.Add(newPlayer);
		}

		private void RemovePlayer(PlayerModel player)
		{
			PlayerView playerView = playersViews.FirstOrDefault(item => item.Model == player);
			playersViews.Add(playerView);

			if (gameModel.CurrentPlayer.Value == player)
				NextPlayer();

			GameObject.Destroy(playerView.gameObject);
		}

		private void NextPlayer()
		{
			PlayerModel nextPlayer = gameModel.Players.NextAfter(gameModel.CurrentPlayer.Value, (a, b) => a == b);

			while (nextPlayer == null || nextPlayer.Hp.Value <= 0)
			{
				nextPlayer = gameModel.Players.NextAfter(nextPlayer, (a, b) => a == b);
			}

			SetCurrentPlayer(nextPlayer);
		}

		private void OnPlayerProjectileInfluencePeak(InfluenceEventArgs args)
		{
			foreach (PlayerView player in playersViews)
			{
				if (Vector3.Distance(args.ProjectileInfluence.transform.position, player.transform.position) < Consts.BOOM_RADIUS)
					player.Model.Hp.Value -= 10;
			}

			terrain.SubstractSphere(args.ProjectileInfluence.transform.position, args.ProjectileInfluence.Radius);
		}

		private void SetCurrentPlayer(PlayerModel playerModel)
        {
			if (playerModel == null)
				return;

			if (gameModel.CurrentPlayer.Value != null)
				gameModel.CurrentPlayer.Value.IsCurrent.Value = false;

			playerModel.IsCurrent.Value = true;
			gameModel.CurrentPlayer.Value = playerModel;
		}
	}
}