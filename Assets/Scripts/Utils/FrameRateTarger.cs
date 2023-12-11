using UnityEngine;

namespace _Project.Scripts.Utils
{
    public class FrameRateTarger : MonoBehaviour
    {
        [SerializeField] private int _frameRate;

        void Start()
        {
            Application.targetFrameRate = _frameRate;
        }
    }
}