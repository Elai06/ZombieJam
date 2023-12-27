using Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.Windows.Header
{
    public class HeaderWindow : Window
    {
        [SerializeField] private HeaderViewInitializer _headerViewInitializer;

        [Inject] private IHeaderUIModel _currenciesModel;

        private void OnEnable()
        {
            _headerViewInitializer.Initialize(_currenciesModel);
        }
    }
}