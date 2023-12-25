using DG.Tweening;
using Gameplay.CinemachineCamera;
using Gameplay.Parking;
using Gameplay.Windows.Gameplay;
using Infrastructure.Windows;
using UnityEngine;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay
{
    public class Finish : MonoBehaviour
    {
        private const int UNIT_LAYER = 3;
        [SerializeField] private ZombieSpawner _zombieSpawner;

        [Inject] private IWindowService _windowService;
        [Inject] private IGameplayModel _gameplayModel;

        [SerializeField] private CameraSelector _cameraSelector;

        private int _zombieCount;

        private void Start()
        {
            InjectService.Instance.Inject(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == UNIT_LAYER)
            {
                _zombieCount++;

                DOVirtual.DelayedCall(0.5f, () => other.gameObject.SetActive(false));

                if (_zombieCount == _zombieSpawner.Zombies.Count)
                {
                    if (_cameraSelector == null)
                    {
                        _cameraSelector = FindObjectOfType<CameraSelector>();
                    }

                    DOVirtual.DelayedCall(1, () =>
                    {
                        _cameraSelector.ChangeCamera(ECameraType.Enemies);
                        _windowService.Open(WindowType.Victory);
                    });
                }
            }
        }
    }
}