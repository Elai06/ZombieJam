using Infrastructure.Windows;
using TMPro;
using UnityEngine;

namespace Gameplay.Windows.Tutorial
{
    public class TutorialWindow : Window
    {
        [SerializeField] private Transform _messageContent;
        [SerializeField] private TextMeshProUGUI _messageText;

        public void SetMessage(string message)
        {
            _messageContent.gameObject.SetActive(true);
            _messageText.text = message;
        }

        private void OnDisable()
        {
            _messageContent.gameObject.SetActive(false);
        }
    }
}