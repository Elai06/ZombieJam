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

        private Image _image;
        public bool IsInteractable;
        public WindowType WindowType;
        public bool IsSelected;

        private void Awake()
        {
            _button = gameObject.GetComponent<Button>();
            _image = gameObject.GetComponent<Image>();
        }

        private void Start()
        {
            _button.interactable = IsInteractable;

            if (IsSelected)
            {
                Selected(true);
            }
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
                _image.color = Color.green;
            }
            else
            {
                gameObject.transform.localScale = new Vector3(1, 1);
                _image.color = Color.white;
            }
        }
    }
}