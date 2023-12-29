using Gameplay.Cards;
using Infrastructure.SceneManagement;

namespace Infrastructure.StateMachine.States
{
    public class LoadLevelState : IState
    {
        private const string Scene = "Gameplay";

        private IStateMachine _stateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly ICardsModel _cardsModel;

        public LoadLevelState(ISceneLoader sceneLoader, ICardsModel cardsModel)
        {
            _sceneLoader = sceneLoader;
            _cardsModel = cardsModel;
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
            _cardsModel.Initialize();

            _stateMachine.Enter<GameState>();
        }

        public void Exit()
        {
        }
    }
}