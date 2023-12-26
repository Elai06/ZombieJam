using System;

namespace Gameplay.Boosters
{
    [Serializable]
    public class BoosterProgressData
    {
        public EBoosterType BoosterType;
        public int Value;

        public BoosterProgressData(EBoosterType boosterType, int value)
        {
            BoosterType = boosterType;
            Value = value;
        }
    }
}