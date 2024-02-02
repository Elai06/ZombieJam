using System;
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Configs.Sprites
{
    [Serializable]
    public struct ParameterIcon
    {
        public EParameter ParameterType;
        public Sprite Icon;
    }
}