﻿using System.Collections.Generic;
using System.Linq;
using Gameplay.Configs.Cards;
using Gameplay.Configs.Zombies;
using Gameplay.Enums;
using Gameplay.Parameters;

namespace Gameplay.Cards
{
    public class CardModel
    {
        public Dictionary<EParameter, float> Parameters { get; } = new();

        public EZombieNames Name { get; private set; }

        public CardModel(CardProgressData progressData, CardsConfigData configData)
        {
            ProgressData = progressData;
            ConfigData = configData;
            InitializeParameters();
            UpdateParamaters();
            Name = configData.ZombieData.Name;
        }

        public CardProgressData ProgressData { get; set; }
        public CardsConfigData ConfigData { get; set; }

        private void InitializeParameters()
        {
            foreach (var parameter in ConfigData.ZombieData.Parameters.Parameters)
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
            foreach (var parameter in ConfigData.ZombieData.Parameters.Parameters)
            {
                float value = parameter.Value;
                for (int i = 0; i < ProgressData.Level; i++)
                {
                    value *= parameter.MultiplierForUpgrade;
                }

                Parameters[parameter.Type] = value;
            }
        }
    }
}