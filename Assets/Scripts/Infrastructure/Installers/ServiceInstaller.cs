using _Project.Scripts.Infrastructure.PersistenceProgress;
using _Project.Scripts.Infrastructure.SaveLoads;
using _Project.Scripts.Infrastructure.StateMachine;
using _Project.Scripts.Infrastructure.StateMachine.States;
using Gameplay.Ad;
using Gameplay.Boosters;
using Gameplay.Cards;
using Gameplay.Configs.Region;
using Gameplay.Curencies;
using Gameplay.InApp;
using Gameplay.Level;
using Gameplay.PlayerTimes;
using Gameplay.RegionMap;
using Gameplay.Reward;
using Gameplay.Shop;
using Gameplay.Tutorial;
using Gameplay.Windows.Boosters;
using Gameplay.Windows.Cards;
using Gameplay.Windows.Gameplay;
using Gameplay.Windows.Header;
using Gameplay.Windows.LevelUp;
using Gameplay.Windows.Rewards;
using Gameplay.Windows.Shop;
using Infrastructure.Events;
using Infrastructure.Input;
using Infrastructure.PersistenceProgress;
using Infrastructure.SaveLoads;
using Infrastructure.SceneManagement;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Infrastructure.StaticData;
using Infrastructure.Timer;
using Infrastructure.UnityBehaviours;
using Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class ServiceInstaller : MonoInstaller
    {
        [SerializeField] private CoroutineService _coroutineService;
        [SerializeField] private LayersContainer _layersContainer;
        [SerializeField] private GameStaticData _gameStaticData;
        [SerializeField] private TimerService _timerService;
        [SerializeField] private SwipeManager _swipeManager;
        [SerializeField] private RegionInitializer _regionInitializer;

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
            Container.BindInterfacesTo<BoostersViewModelFactory>().AsSingle();
            Container.BindInterfacesTo<HeaderViewModelFactory>().AsSingle();
            Container.BindInterfacesTo<RewardViewModelFactory>().AsSingle();
            Container.BindInterfacesTo<CardsViewModelFactory>().AsSingle();
            Container.BindInterfacesTo<ShopViewModelFactory>().AsSingle();
            Container.BindInterfacesTo<LevelUpViewModelFactory>().AsSingle();
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
            Container.Bind<TimerService>().FromInstance(_timerService).AsSingle();
            Container.Bind<SwipeManager>().FromInstance(_swipeManager).AsSingle();
            Container.Bind<RegionInitializer>().FromInstance(_regionInitializer).AsSingle();
        }

        private void BindModels()
        {
            Container.Bind<IGameplayModel>().To<GameplayModel>().AsSingle();
            Container.Bind<IRegionManager>().To<RegionManager>().AsSingle();
            Container.Bind<IBoostersManager>().To<BoostersManager>().AsSingle();
            Container.Bind<ICurrenciesModel>().To<CurrenciesModel>().AsSingle();
            Container.Bind<ILevelModel>().To<LevelModel>().AsSingle();
            Container.Bind<IHeaderUIModel>().To<HeaderUIModel>().AsSingle();
            Container.Bind<IRewardModel>().To<RewardModel>().AsSingle();
            Container.Bind<ICardsModel>().To<CardsModel>().AsSingle();
            Container.Bind<IShopModel>().To<ShopModel>().AsSingle();
            Container.Bind<IAdsService>().To<AdsService>().AsSingle();
            Container.Bind<IInAppService>().To<InAppService>().AsSingle();
            Container.Bind<IPlayerTimesService>().To<PlayerTimesService>().AsSingle();
            Container.Bind<IEventsManager>().To<EventsManager>().AsSingle();
            Container.Bind<ITutorialService>().To<TutorialService>().AsSingle();
        }
    }
}