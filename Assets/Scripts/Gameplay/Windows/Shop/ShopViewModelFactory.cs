using Gameplay.Shop;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Windows.Shop
{
    public class ShopViewModelFactory : IViewModelFactory<ShopViewModel, ShopView, IShopModel>
    {
        private readonly GameStaticData _gamestaticData;

        public ShopViewModelFactory(GameStaticData gamestaticData)
        {
            _gamestaticData = gamestaticData;
        }

        public ShopViewModel Create(IShopModel model, ShopView view)
        {
            return new ShopViewModel(model, view, _gamestaticData);
        }
    }
}