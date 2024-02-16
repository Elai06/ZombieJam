using Gameplay.Tutorial;
using Infrastructure.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Windows.Tutorial
{
    public class TutorialWindow : Window
    {
        [SerializeField] private Image _bg;
        [SerializeField] private RectTransform _messageContent;
        [SerializeField] private TextMeshProUGUI _messageText;

        [Inject] private ITutorialService _tutorialService;

        private void OnEnable()
        {
            _tutorialService.Message += SetMessage;
        }

        private void OnDisable()
        {
            _tutorialService.Message -= SetMessage;

            _messageContent.gameObject.SetActive(false);
        }

        private void SetMessage(string message, Vector2 position, bool isActiveBg)
        {
            _bg.gameObject.SetActive(isActiveBg);
            _messageContent.gameObject.SetActive(true);

            _messageContent.anchoredPosition = position;
            _messageText.text = message;
        }
    }
}