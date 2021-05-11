using System.Collections.Generic;
using UniRx;
using Zenject;
using UnityEngine;
using UnityEngine.UI;
using GlobeTanks.ViewModels;
using System.Linq;
using UnityEngine.SceneManagement;

namespace GlobeTanks.UI
{
	public class GameScreen : MonoBehaviour
	{
		[SerializeField] private Text currentPlayerNameText;
		[SerializeField] private Text shootDirectionAngleText;
		[SerializeField] private Button directionXPlusButton;
		[SerializeField] private Button directionXMinusButton;
		[SerializeField] private Button directionYPlusButton;
		[SerializeField] private Button directionYMinusButton;
		[SerializeField] private Button fireButton;
		[SerializeField] private Button reloadButton;
		[SerializeField] private RectTransform playersContainer;

		private const float CHANGE_ANGLE_STEP = 5f;

		private IFactory<PlayerModel, PlayerUI> playerUIFactory;
		private GameModel gameModel;

		private List<PlayerUI> playerUis = new List<PlayerUI>();
		private CompositeDisposable currentPlayerDisposables = new CompositeDisposable();

		[Inject]
		public void Construct(IFactory<PlayerModel, PlayerUI> playerUIFactory, GameModel gameModel)
		{
			this.playerUIFactory = playerUIFactory;
			this.gameModel = gameModel;

			foreach (PlayerModel player in this.gameModel.Players)
                AddPlayerUI(player);

			gameModel.Players.ObserveAdd()
				.Subscribe(arg => AddPlayerUI(arg.Value))
				.AddTo(this);

			gameModel.Players.ObserveRemove()
				.Subscribe(arg => RemovePlayerUI(arg.Value))
				.AddTo(this);

			directionXPlusButton.OnClickAsObservable()
				.Subscribe(_ => ChangeShootDirectionAngle(new Vector2(1, 0)))
				.AddTo(this);

			directionXMinusButton.OnClickAsObservable()
				.Subscribe(_ => ChangeShootDirectionAngle(new Vector2(-1, 0)))
				.AddTo(this);

			directionYPlusButton.OnClickAsObservable()
				.Subscribe(_ => ChangeShootDirectionAngle(new Vector2(0, 1)))
				.AddTo(this);

			directionYMinusButton.OnClickAsObservable()
				.Subscribe(_ => ChangeShootDirectionAngle(new Vector2(0, -1)))
				.AddTo(this);

			fireButton.OnClickAsObservable()
				.Subscribe(_ => gameModel.CurrentPlayer?.Value.Fire.Execute())
				.AddTo(this);

			reloadButton.OnClickAsObservable()
				.Subscribe(_ => SceneManager.LoadScene(SceneManager.GetActiveScene().name))
				.AddTo(this);

			gameModel.CurrentPlayer
				.Subscribe(playerModel =>
				{
					currentPlayerDisposables.Clear();

					if (playerModel == null)
						return;

					currentPlayerNameText.text = playerModel.Name.Value;
					currentPlayerNameText.color = playerModel.Color.Value;

					playerModel.ShootDirectionAngle
						.Subscribe(angle => shootDirectionAngleText.text = $"({angle.x}; {angle.y})")
						.AddTo(currentPlayerDisposables);
				})
				.AddTo(this);
		}

		private void ChangeShootDirectionAngle(Vector2 direction)
		{
			gameModel.CurrentPlayer.Value.ShootDirectionAngle.Value += CHANGE_ANGLE_STEP * direction;
		}

		private void AddPlayerUI(PlayerModel player)
		{
			PlayerUI playerUi = playerUIFactory.Create(player);
			playerUi.transform.SetParent(playersContainer, false);
			playerUis.Add(playerUi);
		}

		private void RemovePlayerUI(PlayerModel player)
		{
			PlayerUI playerUi = playerUis.FirstOrDefault(item => item.Model == player);
			if (playerUi == null)
				return;

			playerUis.Remove(playerUi);
			Destroy(playerUi.gameObject);
		}
	}
}
