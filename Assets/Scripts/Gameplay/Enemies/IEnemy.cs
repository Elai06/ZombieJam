using Gameplay.Units;
using UnityEngine;
using Utils.CurveBezier;

namespace Gameplay.Enemies
{
    public interface IEnemy
    {
        void GetDamage(float damage);
        Vector3 GetPositionForUnit(Unit unit, float radiusAttack);
        Transform Position { get; }
        bool IsDied { get; }
        void RemoveAttackingUnit(Unit unit);
    }
}