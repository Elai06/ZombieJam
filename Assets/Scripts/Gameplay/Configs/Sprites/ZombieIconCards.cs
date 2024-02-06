using System;
using Gameplay.Configs.Zombies;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Configs.Sprites
{
    [Serializable]
    public struct ZombieIconCards
    {
        public EZombieNames ZombieName;
        public Sprite FullHeighSprite;
        public Sprite HalfHeighSprite;
    }
}