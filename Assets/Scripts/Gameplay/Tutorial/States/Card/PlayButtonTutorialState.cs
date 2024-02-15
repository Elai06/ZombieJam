using Gameplay.Windows.Gameplay;
using Infrastructure.Events;
using Infrastructure.Windows;

namespace Gameplay.Tutorial.States.Card
{
    public class PlayButtonTutorialState : TutorialState
    {
        private IGameplayModel _gameplayModel;

        public PlayButtonTutorialState(ITutorialService tutorialService, IWindowService windowService,
            IEventsManager eventsManager, IGameplayModel gameplayModel) : base(tutorialService, windowService,
            eventsManager, ETutorialState.PlayButton)
        {
            _gameplayModel = gameplayModel;
        }

        public override void Enter()
        {
            base.Enter();

            _gameplayModel.OnStartWave += OnStartWave;
        }

        public override void Exit()
        {
            base.Exit();
            
            _gameplayModel.OnStartWave -= OnStartWave;
        }

        private void OnStartWave(int index)
        {
            _stateMachine.Enter<CompletedTutorialState>();
        }
    }
}