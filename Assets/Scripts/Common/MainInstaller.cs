using Zenject;
using UnityEngine;
using GlobeTanks.UI;
using GlobeTanks.Objects;
using GlobeTanks.ViewModels;

namespace GlobeTanks.Common
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private GameScreen gameScreenPrefab;
        [SerializeField] private PlayerUI playerUIPrefab;
        [SerializeField] private SphereTerrain sphereTerrainPrefab;
        [SerializeField] private ProjectileTracerView projectileTracerViewPrefab;
        [SerializeField] private PlayerView playerViewPrefab;
        [SerializeField] private ProjectileView projectileViewPrefab;
        [SerializeField] private ProjectileInfluence projectileInfluencePrefab;
        [SerializeField] private ShootDirectionView shootDirectionViewPrefab;

        public override void InstallBindings()
        {
            Container.BindWithInrefaces<Startup>();
            Container.BindWithInrefaces<Universe>();

            Container.Bind<GameModel>().AsSingle().NonLazy();
            Container.Bind<MainInputAction>().AsSingle().NonLazy();

            Container.Bind<SphereTerrain>().FromComponentInNewPrefab(sphereTerrainPrefab).AsSingle().NonLazy();
            Container.Bind<ProjectileTracerView>().FromComponentInNewPrefab(projectileTracerViewPrefab).AsSingle().NonLazy();
            Container.Bind<ShootDirectionView>().FromComponentInNewPrefab(shootDirectionViewPrefab).AsSingle().NonLazy();

            Container.Bind<GameScreen>().FromComponentInNewPrefab(gameScreenPrefab).AsSingle().NonLazy();
            
            Container.BindIFactory<PlayerModel, PlayerUI>().FromComponentInNewPrefab(playerUIPrefab).AsSingle().NonLazy();
            Container.BindIFactory<PlayerModel, PlayerView>().FromComponentInNewPrefab(playerViewPrefab).AsSingle().NonLazy();
            Container.BindIFactory<Vector3, float, ProjectileInfluence>().FromComponentInNewPrefab(projectileInfluencePrefab).AsSingle().NonLazy();
            Container.BindIFactory<Vector3, Vector3, float, ProjectileView>().FromComponentInNewPrefab(projectileViewPrefab).AsSingle().NonLazy();
        }
    }
}