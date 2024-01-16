using Infrastructure.Windows;

namespace Gameplay.Tutorial.States
{
    public class CompletedTutorialState : TutorialState
    {
        public CompletedTutorialState(ITutorialService tutorialService,
            IWindowService windowService, ETutorialState state = ETutorialState.Completed)
            : base(tutorialService, windowService, state)
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