using System.Collections.Generic;
using System.Linq;
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Parameters
{
    [CreateAssetMenu(fileName = "ParametersConfig", menuName = "Configs/Unit/ParametersConfig")]
    public class ParametersConfig : ScriptableObject
    {
        [SerializeField] private List<ParameterData> _datas;

        public Dictionary<EParameter, float> GetDictionary()
        {
            return _datas.ToDictionary(data => data.Type, data => data.Value);
        }
    }
}