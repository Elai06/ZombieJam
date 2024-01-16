using Infrastructure.Events;
using Infrastructure.Windows;

namespace Gameplay.Tutorial.States.SwipeState
{
    public class SwipeTutorialState : TutorialState
    {
        public SwipeTutorialState(ITutorialService tutorialService, IWindowService windowService,
            IEventsManager eventsManager, ETutorialState state = ETutorialState.Swipe)
            : base(tutorialService, windowService, eventsManager, state)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}