using Gameplay.Shop;
using Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.Windows.Shop
{
    public class ShopWindow : Window
    {
        [SerializeField] private ShopViewInitializer _shopViewInitializer;
        [Inject] private IShopModel _shopModel;

        private void OnEnable()
        {
            _shopViewInitializer.Initialize(_shopModel);
        }
    }
}