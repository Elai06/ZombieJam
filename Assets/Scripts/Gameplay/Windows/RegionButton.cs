using Infrastructure.Windows;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Windows
{
    public class RegionButton : MonoBehaviour
    {
        private Button _button;

        [Inject] private IWindowService _windowService;

        private void Awake()
        {
            _button = gameObject.GetComponent<Button>();
        }

        private void Start()
        {
            InjectService.Instance.Inject(this);
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(LoadRegion);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(LoadRegion);
        }

        private void LoadRegion()
        {
            SceneManager.LoadScene($"Region");
            
            _windowService.Close(WindowType.MainMenu);
        }
    }
}