namespace Gameplay.Tutorial.States
{
    public class CardTutorialState : TutorialState
    {
        public CardTutorialState(ITutorialService tutorialService,
            ETutorialState state = ETutorialState.Card)
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