using System;
using Gameplay.Boosters;
using UnityEngine;

namespace Gameplay.Configs.Sprites
{
    [Serializable]
    public struct BoosterIcon
    {
        public EBoosterType BoosterType;
        public Sprite Sprite;
    }
}