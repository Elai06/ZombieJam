using System;
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Enemies
{
    public interface IEnemy
    {
        void GetDamage(float damage, bool isNeedBlood = true);
        Transform Transform { get; }
        bool IsDied { get; }
         event Action TakeDamage;
         EEnemyType EnemyType { get; }
    }
}