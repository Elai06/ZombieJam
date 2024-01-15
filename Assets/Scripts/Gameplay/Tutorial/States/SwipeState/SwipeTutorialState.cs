namespace Gameplay.Tutorial.States
{
    public class SwipeTutorialState : TutorialState
    {
        public SwipeTutorialState(ITutorialService tutorialService,
            ETutorialState state = ETutorialState.Swipe)
            : base(tutorialService, state)
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