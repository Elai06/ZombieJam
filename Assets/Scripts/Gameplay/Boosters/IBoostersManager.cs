using System;

namespace Gameplay.Boosters
{
    public interface IBoostersManager
    {
        void ActivateBooster(EBoosterType boosterType);
        BoosterProgressData GetBoosterProgressData(EBoosterType eBoosterType);
        event Action<EBoosterType> Activate;
        void ConsumeBooster(EBoosterType boosterType, int value);
    }
}