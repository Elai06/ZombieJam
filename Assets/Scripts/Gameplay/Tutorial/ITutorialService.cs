using System;
using Gameplay.Enums;

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
    }
}