using Gameplay.Windows.Gameplay;
using UnityEngine;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay
{
    public class CursorTrail : MonoBehaviour
    {
        [SerializeField] Camera _cam;
        [SerializeField] float _distanceFromCamera = 1;
        [SerializeField] private TrailRenderer _trailRenderer;

        [Inject] private IGameplayModel _gameplayModel;
        
        private void Start()
        {
            InjectService.Instance.Inject(this);
        }

        private void Update()
        {
            if (!_gameplayModel.IsStartWave)
            {
                if (_trailRenderer.gameObject.activeSelf)
                {
                    _trailRenderer.gameObject.SetActive(false);
                }
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (!_trailRenderer.gameObject.activeSelf)
                {
                    _trailRenderer.gameObject.SetActive(true);
                }

                UpdateTrailPosition();
                _trailRenderer.Clear();
            }

            if (Input.GetMouseButton(0))
            {
                UpdateTrailPosition();
            }
        }

        private void UpdateTrailPosition()
        {
            Vector3 mousePosition = Input.mousePosition;
            _trailRenderer.transform.position = _cam.ViewportToWorldPoint(new Vector3(mousePosition.x / Screen.width,
                mousePosition.y / Screen.height, _distanceFromCamera));
        }
    }
}