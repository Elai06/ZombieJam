using System;

namespace Gameplay.Tutorial
{
    public interface ITutorialService
    {
        void SetState(ETutorialState tutorialState);
        ETutorialState CurrentState { get; }
        event Action<ETutorialState> СhangedState;
        void Initalize();
    }
}