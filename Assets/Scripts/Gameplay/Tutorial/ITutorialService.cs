using System;
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Tutorial
{
    public interface ITutorialService
    {
        void SetState(ETutorialState tutorialState);
        ETutorialState CurrentState { get; }
        event Action<ETutorialState> СhangedState;
        void Initalize();
        event Action<EUnitClass> OnOpenCardPopUp;
        void OpenCardPopUp(EUnitClass unitClass);
        void StartFinishCardTutorial();
        event Action<string, Vector2, bool> Message;
        void ShowMessage(string message, Vector2 messagePosition,bool isActiveBg);
    }
}