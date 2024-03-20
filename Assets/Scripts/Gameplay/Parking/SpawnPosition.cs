using System;
using System.Collections.Generic;
using Gameplay.Configs.Zombies;
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Parking
{
    public class SpawnPosition : MonoBehaviour
    {
        [SerializeField] private ESwipeSide _eSwipeSide;
        public EZombieNames Name;
        public EZombieSize ZombieSize;
        [SerializeField] private List<SpawnPosition> _cooperativePosition = new();

        private Color _color;

        public bool IsCooperative { get; private set; }

        public List<SpawnPosition> CooperativePositions => _cooperativePosition;


        public ESwipeSide SwipeSide => _eSwipeSide;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_cooperativePosition.Count > 0 && ZombieSize == EZombieSize.TwoCells)
            {
                CooperativePosition();
                Gizmos.color = Color.blue;

                foreach (var position in _cooperativePosition)
                {
                    Gizmos.color = Color.blue;
                    position.CooperativePosition();
                    position.ZombieSize = ZombieSize;
                    position.Name = Name;
                }
            }

            SetColor();

            var gizmosPosition = transform.position;
            var gizmosScale = ZombieSize == EZombieSize.SingleCell
                ? new Vector3(0.85f, 0.2f, 0.85f)
                : new Vector3(1f, 0.2f, 1f);
            gizmosPosition.y += 0.1f;
            Gizmos.DrawCube(gizmosPosition, gizmosScale);

            if (_cooperativePosition.Count > 0)
            {
                var coopPos = _cooperativePosition[0].transform.position;
                coopPos.y += 0.25f;
                gizmosPosition.y += 0.15f;
                Gizmos.DrawLine(gizmosPosition, coopPos);
            }
        }

        private void CooperativePosition()
        {
            if (_cooperativePosition.Count == 0 && ZombieSize == EZombieSize.TwoCells)
            {
                _eSwipeSide = ESwipeSide.None;
            }

            IsCooperative = true;
        }
#endif

        public bool IsAvailablePosition()
        {
            return _eSwipeSide != ESwipeSide.None;
        }

        public Vector3 GetSpawnPosition()
        {
            if (ZombieSize == EZombieSize.TwoCells)
            {
                return (transform.position + _cooperativePosition[^1].transform.position) / 2;
            }

            return transform.position;
        }

        private void SetColor()
        {
            if (_eSwipeSide == ESwipeSide.None && (ZombieSize == EZombieSize.SingleCell || _cooperativePosition.Count > 0))
            {
                _color = Color.red;
                Gizmos.color = _color;
                return;
            }

            switch (Name)
            {
                case EZombieNames.Hitchhiker:
                    _color = Color.cyan;
                    break;
                case EZombieNames.Zombie:
                    _color = Color.cyan;
                    break;
                case EZombieNames.BrainThrower:
                    _color = Color.blue;
                    break;
                case EZombieNames.WalkingCoffin:
                    _color = Color.green;
                    break;
                case EZombieNames.ArmoredZombie:
                    _color = Color.green;
                    break;
                case EZombieNames.ZombieScooter:
                    _color = Color.green;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Gizmos.color = _color;
        }
    }
}