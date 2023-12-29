using System.Collections.Generic;
using System.Linq;
using Gameplay.Configs.Cards;
using Gameplay.Enums;
using Gameplay.Parameters;

namespace Gameplay.Cards
{
    public class CardModel
    {
        public Dictionary<EParameter, float> Parameters { get; } = new();

        public CardModel(CardProgressData progressData, CardsConfigData configData)
        {
            ProgressData = progressData;
            ConfigData = configData;
            InitializeParameters();
            UpdateParamaters();
        }

        public CardProgressData ProgressData { get; set; }
        public CardsConfigData ConfigData { get; set; }

        private void InitializeParameters()
        {
            foreach (var parameter in ConfigData.ParametersConfig.Parameters)
            {
                Parameters.Add(parameter.Type, parameter.Value);
            }
        }

        public void Upgrade()
        {
            ProgressData.Level++;
            UpdateParamaters();
        }

        private void UpdateParamaters()
        {
            foreach (var parameter in ConfigData.ParametersConfig.GetDictionary().Values)
            {
                float value = parameter.Value;
                for (int y = 0; y < ProgressData.Level; y++)
                {
                    value = Parameters[parameter.Type] * parameter.MultiplierForUpgrade;
                }
                
                Parameters[parameter.Type] = value;
            }
        }
    }
}