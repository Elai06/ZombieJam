using Gameplay.Boosters;
using Gameplay.Cards;
using Gameplay.PlayerTimes;
using Gameplay.RegionMap;
using Gameplay.Tutorial;
using Gameplay.Windows.Gameplay;
using Infrastructure.Events;
using Infrastructure.Input;
using Infrastructure.SceneManagement;
using UnityEngine;

namespace Infrastructure.StateMachine.States
{
    public class LoadLevelState : IState
    {
        private const string Scene = "Gameplay";

        private IStateMachine _stateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly ICardsModel _cardsModel;
        private readonly IPlayerTimesService _playerTimesService;
        private readonly IEventsManager _eventsManager;
        private readonly ITutorialService _tutorialService;
        private readonly SwipeManager _swipeManager;
        private readonly IGameplayModel _gameplayModel;
        private readonly IBoostersManager _boostersManager;

        public LoadLevelState(ISceneLoader sceneLoader, ICardsModel cardsModel, IPlayerTimesService playerTimesService,
            IEventsManager eventsManager, ITutorialService tutorialService, SwipeManager swipeManager,
            IGameplayModel gameplayModel, IBoostersManager boostersManager)
        {
            _sceneLoader = sceneLoader;
            _cardsModel = cardsModel;
            _playerTimesService = playerTimesService;
            _eventsManager = eventsManager;
            _tutorialService = tutorialService;
            _swipeManager = swipeManager;
            _gameplayModel = gameplayModel;
            _boostersManager = boostersManager;
        }

        public void Enter()
        {
            _sceneLoader.Load(Scene, OnLoaded);
        }

        public void Initialize(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        private void OnLoaded()
        {
            _eventsManager.Initialize(_gameplayModel);
            _playerTimesService.SetDaysInPlay();
            _cardsModel.Initialize();
            _tutorialService.Initialize(_gameplayModel);
            _swipeManager.Initialize();
            _boostersManager.Initialize(_gameplayModel);

            AppMetrica.Instance.ReportEvent("Game started",
                $"{{\"Project version\":\"{Application.version}\\, Day\":\"{_playerTimesService.GetDaysInPlay()}\"}}");

            _stateMachine.Enter<GameState>();
        }

        public void Exit()
        {
        }
    }
}