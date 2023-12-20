using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.StaticData
{
    [CreateAssetMenu(fileName = "ParkingConfig", menuName = "Configs/ParkingConfig")]

    public class ParkingConfig : ScriptableObject
    {
        [SerializeField] private List<GameObject> _parkings;

        public List<GameObject> Parkings => _parkings;
    }
}