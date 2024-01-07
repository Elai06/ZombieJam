using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Configs.Shop
{
    [CreateAssetMenu(menuName = "Configs/ShopConfig", fileName = "ShopConfig", order = 0)]
    public class ShopConfig : ScriptableObject
    {
        [SerializeField] private List<ShopConfigData> _shopConfigData;

        public List<ShopConfigData> ConfigData => _shopConfigData;
    }
}