using Infrastructure.Windows;

namespace Gameplay.Tutorial.States
{
    public class CardTutorialState : TutorialState
    {
        public CardTutorialState(ITutorialService tutorialService,IWindowService windowService,
            ETutorialState state = ETutorialState.Card)
            : base(tutorialService, state, windowService)
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