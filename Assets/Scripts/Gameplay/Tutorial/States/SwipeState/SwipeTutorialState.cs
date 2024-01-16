﻿using Infrastructure.Windows;

namespace Gameplay.Tutorial.States.SwipeState
{
    public class SwipeTutorialState : TutorialState
    {
        public SwipeTutorialState(ITutorialService tutorialService, IWindowService windowService,
            ETutorialState state = ETutorialState.Swipe)
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