using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class EnemyUnitObstacleAvoidance : ObstacleAvoidance
    {
        public void StartMovement(Transform target, float radiusAttack, Transform diedZone)
        {
            /*
            if (target.transform.position.z < diedZone.position.z)
            {
                StopMove();
                return;
            }
            */
            
            StartMovement(target, radiusAttack);
        }
    }
}