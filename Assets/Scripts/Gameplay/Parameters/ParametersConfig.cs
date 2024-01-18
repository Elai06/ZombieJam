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

        public List<ParameterData> Parameters => _datas;

        public Dictionary<EParameter, ParameterData> GetDictionary()
        {
            return _datas.ToDictionary(data => data.Type, data => data);
        }

        public Dictionary<EParameter, float> GetDictionaryTypeFloat()
        {
            return _datas.ToDictionary(data => data.Type, data => data.Value);
        }
    }
}