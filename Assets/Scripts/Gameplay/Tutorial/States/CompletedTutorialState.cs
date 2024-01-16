using Infrastructure.Events;
using Infrastructure.Windows;

namespace Gameplay.Tutorial.States
{
    public class CompletedTutorialState : TutorialState
    {
        public CompletedTutorialState(ITutorialService tutorialService,
            IWindowService windowService, IEventsManager eventsManager, ETutorialState state = ETutorialState.Completed)
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