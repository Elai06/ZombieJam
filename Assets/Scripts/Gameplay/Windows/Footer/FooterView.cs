using System.Collections.Generic;
using System.Linq;
using Infrastructure.Windows;
using UnityEngine;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Windows.Footer
{
    public class FooterView : MonoBehaviour
    {
        [SerializeField] private List<FooterTab> _footerTabs = new();
        [Inject] private IWindowService _windowService;

        private void Start()
        {
            InjectService.Instance.Inject(this);
        }

        private void OnEnable()
        {
            foreach (var footerTab in _footerTabs)
            {
                footerTab.Click += SelectedTab;
            }

            if (_windowService != null)
            {
                _windowService.OnOpen += OpenView;
            }
        }

        private void OpenView(WindowType viewType)
        {
            var isFooterView = false;
            foreach (var tab in _footerTabs.Where(x => x.IsInteractable))
            {
                if (tab.WindowType == viewType)
                {
                    isFooterView = true;
                }
            }

            if (!isFooterView) return;

            foreach (var tab in _footerTabs.Where(x => x.IsInteractable))
            {
                tab.Selected(tab.WindowType == viewType);
            }
        }

        private void OnDisable()
        {
            foreach (var footerTab in _footerTabs)
            {
                footerTab.Click -= SelectedTab;
            }

            _windowService.OnOpen -= OpenView;
        }

        private void SelectedTab(FooterTab selected)
        {
            foreach (var footerTab in _footerTabs.Where(footerTab => footerTab.IsInteractable))
            {
                if (footerTab.WindowType != selected.WindowType)
                {
                    continue;
                }

                _windowService.Open(selected.WindowType);
            }
        }
    }
}