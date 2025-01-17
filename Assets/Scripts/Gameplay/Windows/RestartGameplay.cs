﻿using System;
using Gameplay.Tutorial;
using Gameplay.Windows.Gameplay;
using Infrastructure.Windows;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Windows
{
    public class RestartGameplay : MonoBehaviour
    {
        private Button _button;

        [Inject] private IWindowService _windowService;
        [Inject] private ITutorialService _tutorialService;
        [Inject] private IGameplayModel _gameplayModel;

        private void OnEnable()
        {
            if (_button == null)
            {
                _button = gameObject.GetComponent<Button>();
            }

            _button.onClick.AddListener(Restart);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Restart);
        }

        private void Restart()
        {
            if (_tutorialService.CurrentState == ETutorialState.Swipe) return;

            SceneManager.UnloadSceneAsync("Gameplay");
            SceneManager.LoadScene($"Gameplay");

            if (_windowService.IsOpen(WindowType.Died))
            {
                _windowService.Close(WindowType.Died);
            }

            _windowService.Open(WindowType.MainMenu);
            _windowService.Open(WindowType.Footer);
            _gameplayModel.StopWave();
        }
    }
}