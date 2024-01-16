using Gameplay.Shop;
using Gameplay.Tutorial;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Windows.Shop
{
    public class ShopViewModelFactory : IViewModelFactory<ShopViewModel, ShopView, IShopModel>
    {
        private readonly GameStaticData _gamestaticData;
        private readonly ITutorialService _tutorialService;

        public ShopViewModelFactory(GameStaticData gamestaticData, ITutorialService tutorialService)
        {
            _gamestaticData = gamestaticData;
            _tutorialService = tutorialService;
        }

        public ShopViewModel Create(IShopModel model, ShopView view)
        {
            return new ShopViewModel(model, view, _gamestaticData, _tutorialService);
        }
    }
}