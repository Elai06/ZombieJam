using Gameplay.Enemies;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Battle
{
    public interface ITargetManager
    {
        Unit GetTargetUnit(Transform buildingTransform, float radiusAttack);
        EnemyTower GetTargetEnemy(Transform buildingTransform);
    }
}