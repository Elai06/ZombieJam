using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Configs
{
    [CreateAssetMenu(fileName = "ZombieConfig", menuName = "Configs/ZombieConfig")]
    public class ZombieConfig : ScriptableObject
    {
        [SerializeField] private List<ZombieData> _config;

        public List<ZombieData> Config => _config;
    }
}