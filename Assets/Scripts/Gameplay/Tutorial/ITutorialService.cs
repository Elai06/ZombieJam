﻿using System;
using Gameplay.Enums;

namespace Gameplay.Tutorial
{
    public interface ITutorialService
    {
        void SetState(ETutorialState tutorialState);
        ETutorialState CurrentState { get; }
        event Action<ETutorialState> СhangedState;
        void Initalize();
        void SwipeStateCompleted();
        event Action<EZombieType> OnOpenCardPopUp;
        void OpenCardPopUp(EZombieType zombieType);
    }
}