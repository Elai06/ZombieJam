using Gameplay.Enums;
using UnityEngine;
using UnityEngine.UI;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Tutorial.States.Card
{
    public class OpenPopUpCardTutorial : MonoBehaviour
    {
        private Button _button;

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
            _button.onClick.AddListener(UpgradeCard);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(UpgradeCard);
        }

        private void UpgradeCard()
        {
            _tutorialService.OpenCardPopUp(EZombieType.Easy);
        }
    }
}