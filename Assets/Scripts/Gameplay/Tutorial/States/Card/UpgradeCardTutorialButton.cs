using Gameplay.Cards;
using Gameplay.Configs.Zombies;
using Gameplay.Enums;
using UnityEngine;
using UnityEngine.UI;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Tutorial.States.Card
{
    public class UpgradeCardTutorialButton : MonoBehaviour
    {
        private Button _button;

        [Inject] private ICardsModel _cardsModel;

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
            _button.onClick.AddListener(UpgradeCard);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(UpgradeCard);
        }

        private void UpgradeCard()
        {
            _cardsModel.UpgradeZombie(EZombieNames.Zombie);
        }
    }
}