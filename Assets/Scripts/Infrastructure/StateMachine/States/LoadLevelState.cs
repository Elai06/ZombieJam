using Gameplay.Cards;
using Gameplay.PlayerTimes;
using Infrastructure.Events;
using Infrastructure.SceneManagement;

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

        public LoadLevelState(ISceneLoader sceneLoader, ICardsModel cardsModel, IPlayerTimesService playerTimesService,
            IEventsManager eventsManager)
        {
            _sceneLoader = sceneLoader;
            _cardsModel = cardsModel;
            _playerTimesService = playerTimesService;
            _eventsManager = eventsManager;
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
            _eventsManager.Initialize();
            _playerTimesService.SetDaysInPlay();
            _cardsModel.Initialize();

            AppMetrica.Instance.ReportEvent("Game started", $"Day {_playerTimesService.GetDaysInPlay()}");

            _stateMachine.Enter<GameState>();
        }

        public void Exit()
        {
        }
    }
}