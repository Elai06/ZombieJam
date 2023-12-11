using Gameplay.Enums;
using UnityEngine;

public class SpawnPosition : MonoBehaviour
{
    [SerializeField] private bool _isAvailable = true;
    [SerializeField] private ESwipeDirection _eSwipeDirection;

    private void OnDrawGizmos()
    {
        Gizmos.color = _isAvailable ? Color.green : Color.red;
        Gizmos.DrawCube(transform.position, new Vector3(0.9f, 0.2f, 0.9f));
    }

    public bool IsAvailablePosition()
    {
        return _isAvailable;
    }

    public ESwipeDirection GetSwipeDirection()
    {
        return _eSwipeDirection;
    }
}