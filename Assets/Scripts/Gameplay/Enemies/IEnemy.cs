using Gameplay.Units;
using UnityEngine;
using Utils.CurveBezier;

namespace Gameplay.Enemies
{
    public interface IEnemy
    {
        void GetDamage(float damage);
        Transform Transform { get; }
        bool IsDied { get; }
        void RemoveAttackingUnit(Unit unit);
         Color BloodColor { get; }
    }
}