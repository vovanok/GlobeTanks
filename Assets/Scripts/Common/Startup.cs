using UnityEngine;
using Zenject;
using GlobeTanks.ViewModels;

namespace GlobeTanks.Common
{
	public class Startup : IInitializable
	{
		private readonly SphereTerrain sphereTerrain;
		private readonly GameModel gameModel;
		private readonly Universe universe;

		public Startup(SphereTerrain sphereTerrain, GameModel gameModel, Universe universe)
		{
			this.sphereTerrain = sphereTerrain;
			this.gameModel = gameModel;
			this.universe = universe;
		}

		public void Initialize()
		{
			gameModel.Players.Clear();

			AddPlayer(1, sphereTerrain.GetPointOnSurface(Vector3.right) * 1.1f, new Vector3(0, 0, -90));
            AddPlayer(2, sphereTerrain.GetPointOnSurface(Vector3.left) * 1.1f, new Vector3(0, 0, 90));
            AddPlayer(3, sphereTerrain.GetPointOnSurface(Vector3.up) * 1.1f, Vector3.zero);
            AddPlayer(4, sphereTerrain.GetPointOnSurface(Vector3.down) * 1.1f, new Vector3(0, -90, 180));

			universe.StartGame();
		}

		private void AddPlayer(int number, Vector3 initialPosition, Vector3 initialRotation)
		{
			PlayerModel newPlayer = new PlayerModel(initialPosition, initialRotation);
			newPlayer.Name.Value = $"Player {number}";
			newPlayer.Color.Value = new Color(Random.value, Random.value, Random.value, 1.0f);

			gameModel.Players.Add(newPlayer);
		}
	}
}
