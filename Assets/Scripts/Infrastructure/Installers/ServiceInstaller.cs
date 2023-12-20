using _Project.Scripts.Infrastructure.PersistenceProgress;
using _Project.Scripts.Infrastructure.SaveLoads;
using _Project.Scripts.Infrastructure.StateMachine;
using _Project.Scripts.Infrastructure.StateMachine.States;
using Gameplay.Windows.Gameplay;
using Infrastructure.PersistenceProgress;
using Infrastructure.SaveLoads;
using Infrastructure.SceneManagement;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Infrastructure.StaticData;
using Infrastructure.UnityBehaviours;
using Infrastructure.Windows;
using SirGames.Scripts.Infrastructure.Windows;
using SirGames.Scripts.Windows;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class ServiceInstaller : MonoInstaller
    {
        [SerializeField] private CoroutineService _coroutineService;
        [SerializeField] private LayersContainer _layersContainer;
        [SerializeField] private GameStaticData _gameStaticData;

        public override void InstallBindings()
        {
            BindViewModelFactory();
            BindGameStates();
            BindInfrastructureServices();
            BindModels();
        }

        private void BindViewModelFactory()
        {
            Container.BindInterfacesTo<GameplayViewModelFactory>().AsSingle();
        }

        private void BindGameStates()
        {
            Container.Bind<IStateMachine>().To<GameStateMachine>().AsSingle();
            Container.Bind<ExitState>().AsSingle();
            Container.Bind<GameState>().AsSingle();
            Container.Bind<LoadLevelState>().AsSingle();
            Container.Bind<BootstrapState>().AsSingle();
        }

        private void BindInfrastructureServices()
        {
            Container.Bind<IProgressService>().To<PlayerProgressService>().AsSingle();
            Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
            Container.Bind<IWindowService>().To<WindowService>().AsSingle();
            Container.Bind<IWindowFactory>().To<WindowFactory>().AsSingle();

            Container.Bind<LayersContainer>().FromInstance(_layersContainer).AsSingle();
            Container.Bind<GameStaticData>().FromInstance(_gameStaticData).AsSingle();
            Container.Bind<ICoroutineService>().FromInstance(_coroutineService).AsSingle();
        }

        private void BindModels()
        {
            Container.Bind<IGameplayModel>().To<GameplayModel>().AsSingle();

        }
    }
}