using System;
using Infrastructure.Windows;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Windows.Footer
{
    public class RegionView : MonoBehaviour
    {
        [Inject] private IWindowService _windowService;

        private void Start()
        {
            InjectService.Instance.Inject(this);
            
            LoadRegion();
        }

        private void OnEnable()
        {
            if (_windowService != null)
            {
                LoadRegion();
            }
        }

        private void LoadRegion()
        {
            _windowService.Close(WindowType.MainMenu);
            _windowService.Close(WindowType.Gameplay);
            
            SceneManager.LoadScene($"Region");
        }
    }
}