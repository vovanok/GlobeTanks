using UnityEngine;
using UniRx;

namespace GlobeTanks.ViewModels
{
    public class PlayerModel
    {
        public ReactiveProperty<string> Name { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<bool> IsCurrent { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<int> Hp { get; } = new ReactiveProperty<int>();
        public ReactiveCommand Fire { get; } = new ReactiveCommand();
        public ReactiveProperty<Vector2> ShootDirectionAngle { get; } = new ReactiveProperty<Vector2>();
        public ReactiveProperty<Color> Color { get; } = new ReactiveProperty<Color>();
        public Vector3 InitialPosition { get; private set; }
        public Vector3 InitialRotation { get; private set; }

        public PlayerModel(Vector3 initialPosition, Vector3 initialRotation)
        {
            InitialPosition = initialPosition;
            InitialRotation = initialRotation;

            Hp.Value = 100;

            Hp
                .Subscribe(value =>
                {
                    if (value < 0)
                        Hp.Value = 0;

                    if (value > 100)
                        Hp.Value = 100;
                });

            ShootDirectionAngle
                .Subscribe(value =>
                {
                    Vector2 correctedValue = value;

                    if (correctedValue.x < -90)
                        correctedValue.x = -90;

                    if (correctedValue.x > 90)
                        correctedValue.x = 90;

                    if (correctedValue.y < -90)
                        correctedValue.y = -90;

                    if (correctedValue.y > 90)
                        correctedValue.y = 90;

                    if (correctedValue != value)
                        ShootDirectionAngle.Value = correctedValue;
                });
        }
    }
}