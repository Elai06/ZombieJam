using System.Collections.Generic;
using Gameplay;
using Gameplay.Enums;
using UnityEngine;

public class SpawnPosition : MonoBehaviour
{
    [SerializeField] private bool _isAvailable = true;
    [SerializeField] private ESwipeDirection _eSwipeDirection;
    public EZombieType ZombieType;
    [SerializeField] private List<SpawnPosition> _cooperativePosition = new();

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = _isAvailable ? Color.green : Color.red;
        Gizmos.DrawCube(transform.position, new Vector3(0.9f, 0.2f, 0.9f));
    }
#endif

    public bool IsAvailablePosition()
    {
        return _isAvailable;
    }

    public ESwipeDirection GetSwipeDirection()
    {
        return _eSwipeDirection;
    }

    public Vector3 GetSpawnPosition()
    {
        if (_cooperativePosition.Count != 0)
        {
            return (transform.position + _cooperativePosition[^1].transform.position ) / 2;
        }

        return transform.position;
    }
}