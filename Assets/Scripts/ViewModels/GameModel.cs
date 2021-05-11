using UniRx;

namespace GlobeTanks.ViewModels
{
    public class GameModel
    {
        public ReactiveProperty<PlayerModel> CurrentPlayer { get; } = new ReactiveProperty<PlayerModel>();
        public ReactiveCollection<PlayerModel> Players { get; } = new ReactiveCollection<PlayerModel>();
    }
}
