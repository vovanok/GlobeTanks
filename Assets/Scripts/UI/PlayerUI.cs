using System.Text;
using UniRx;
using Zenject;
using UnityEngine;
using UnityEngine.UI;
using GlobeTanks.ViewModels;

namespace GlobeTanks.UI
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private Text infoText;
        [SerializeField] private Image playerColor;

        public PlayerModel Model { get; private set; }

        [Inject]
        public void Construct(PlayerModel player)
        {
            Model = player;

            playerColor.color = player.Color.Value;

            player.Name
                .Subscribe(_ => RefreshInfoText())
                .AddTo(this);

            player.IsCurrent
                .Subscribe(_ => RefreshInfoText())
                .AddTo(this);

            player.Hp
                .Subscribe(_ => RefreshInfoText())
                .AddTo(this);

            RefreshInfoText();
        }

        private void RefreshInfoText()
        {
            StringBuilder sb = new StringBuilder();

            if (Model.IsCurrent.Value)
                sb.Append("> ");

            sb.Append($"{Model.Name.Value} ({Model.Hp.Value})");

            if (Model.Hp.Value <= 0)
                sb.Append(" - DIED");

            infoText.text = sb.ToString();
        }
    }
}
