namespace Gameplay.Tutorial.States
{
    public class ShopTutorialState : TutorialState
    {
        public ShopTutorialState(ITutorialService tutorialService,
            ETutorialState state = ETutorialState.Shop)
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