using Gameplay.Enums;
using Gameplay.Shop;
using UnityEngine;
using UnityEngine.UI;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Tutorial.States.Shop
{
    public class GetSimpleBoxTutorialButton : MonoBehaviour
    {
        private Button _button;

        [Inject] private IShopModel _shopModel;

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
            _shopModel.GetTutorialRewards(EShopProductType.SimpleBox);
        }
    }
}