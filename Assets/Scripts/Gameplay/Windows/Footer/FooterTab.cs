using System;
using Infrastructure.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Windows.Footer
{
    public class FooterTab : MonoBehaviour
    {
        public event Action<FooterTab> Click;

        private Button _button;

        [SerializeField] private WindowType _windowType;
        [SerializeField] private Image _lockImage;

        public bool IsInteractable;
        public bool IsSelected;

        public WindowType WindowType => _windowType;

        private void Awake()
        {
            _button = gameObject.GetComponent<Button>();

            _button.interactable = IsInteractable;

            _lockImage.gameObject.SetActive(!IsInteractable);
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(ClickButton);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(ClickButton);
        }

        private void ClickButton()
        {
            Click?.Invoke(this);
        }

        public void Selected(bool isSelected)
        {
            IsSelected = isSelected;

            if (IsSelected)
            {
                gameObject.transform.localScale = new Vector3(1.15f, 1.15f);
            }
            else
            {
                gameObject.transform.localScale = new Vector3(1, 1);
            }
        }

        public void SetInteractable(bool isInteractable)
        {
            if (IsInteractable == false) return;

            _button.interactable = isInteractable;
        }

        public void SetImage(Sprite image)
        {
            if (_button.interactable)
            {
                _button.image.sprite = image;
            }
        }
    }
}