using Gameplay.Enums;
using Gameplay.Windows.Gameplay;
using Infrastructure.Events;
using Infrastructure.Windows;
using UnityEngine;

namespace Gameplay.Tutorial.States.Card
{
    public class PlayButtonTutorialState : TutorialState
    {
        private IGameplayModel _gameplayModel;

        private Vector2 _messagePosition = new(0, -2150);

        private const string MESSAGE = "To battle!";
        
        public PlayButtonTutorialState(ITutorialService tutorialService, IWindowService windowService,
            IEventsManager eventsManager, IGameplayModel gameplayModel) : base(tutorialService, windowService,
            eventsManager, ETutorialState.PlayButton)
        {
            _gameplayModel = gameplayModel;
        }

        public override void Enter()
        {
            base.Enter();

            _windowService.Open(WindowType.Tutorial);
            _tutorialService.ShowMessage(MESSAGE, _messagePosition, false);
            
            _gameplayModel.OnStartWave += OnStartWave;
        }

        public override void Exit()
        {
            base.Exit();
            
            _gameplayModel.OnStartWave -= OnStartWave;
        }

        private void OnStartWave(ERegionType regionType ,int index)
        {
            _windowService.Close(WindowType.Tutorial);
            _stateMachine.Enter<CompletedTutorialState>();
        }
    }
}