using System;
using UnityEngine;

namespace Gameplay.Enemies
{
    public interface IEnemy
    {
        void GetDamage(float damage);
        Transform Transform { get; }
        bool IsDied { get; }
         Color BloodColor { get; }
         event Action TakeDamage;
    }
}