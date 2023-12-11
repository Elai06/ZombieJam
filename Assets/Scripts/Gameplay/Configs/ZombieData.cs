using System;
using UnityEngine;

namespace Gameplay.Configs
{
    [Serializable]
    public struct ZombieData
    {
        public EZombieType Type;
        public GameObject Prefab;
    }
}