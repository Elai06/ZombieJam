using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Configs.Cards
{
    [CreateAssetMenu(fileName = "CardsConfig", menuName = "Configs/CardsConfig")]
    public class CardsConfig : ScriptableObject
    {
        [SerializeField] private List<CardsConfigData> _cardsConfig;

        public List<CardsConfigData> Cards => _cardsConfig;
    }
}