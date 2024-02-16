using Gameplay.Enums;
using Gameplay.Tutorial.States.Shop.Box;
using Gameplay.Windows.Gameplay;
using Infrastructure.Events;
using Infrastructure.Windows;

namespace Gameplay.Tutorial.States.SwipeState
{
    public class SwipeTutorialState : TutorialState
    {
        private IGameplayModel _gameplayModel;

        public SwipeTutorialState(ITutorialService tutorialService, IWindowService windowService,
            IEventsManager eventsManager, IGameplayModel gameplayModel, ETutorialState state = ETutorialState.Swipe)
            : base(tutorialService, windowService, eventsManager, state)
        {
            _gameplayModel = gameplayModel;
        }

        public override void Enter()
        {
            base.Enter();

            _gameplayModel.OnWaveCompleted += WaveCompleted;
         //   _windowService.Open(WindowType.Tutorial);
        }

        public override void Exit()
        {
            base.Exit();

            _gameplayModel.OnWaveCompleted -= WaveCompleted;
        }

        private void WaveCompleted(ERegionType regionType, int waveIndex)
        {
            if (regionType == ERegionType.SurvivorsHaven && waveIndex == 1)
            {
                _stateMachine.Enter<ShopBoxTutorialState>();
            }
        }
    }
}