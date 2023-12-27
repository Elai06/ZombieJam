using Gameplay.Curencies;
using Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.Windows.Header
{
    public class HeaderWindow : Window
    {
        [SerializeField] private HeaderViewInitializer _headerViewInitializer;

        [Inject] private ICurrenciesModel _currenciesModel;

        private void OnEnable()
        {
            _headerViewInitializer.Initialize(_currenciesModel);
        }
    }
}