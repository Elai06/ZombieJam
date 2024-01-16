using Gameplay.Enums;
using Gameplay.Shop;
using UnityEngine;
using UnityEngine.UI;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Tutorial.States.Shop
{
    public class PurchaseProductTutorialButton : MonoBehaviour
    {
        private Button _button;

        [Inject] private IShopModel _shopModel;
        [Inject] private ITutorialService _tutorialService;

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
            _button.onClick.AddListener(GetReward);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(GetReward);
        }

        private void GetReward()
        {
            var productType = _tutorialService.CurrentState == ETutorialState.ShopBox
                ? EShopProductType.SimpleBox
                : EShopProductType.LittleSoft;

            _shopModel.TutorialPurchase(productType);
        }
    }
}