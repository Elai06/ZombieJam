using System;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using Infrastructure.UnityBehaviours;
using Unity.Mathematics;
using UnityEngine;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay
{
    public class LevelManager : MonoBehaviour
    {
        private IProgressService _progressService;
        private GameStaticData _gameStatic;

        private GameObject _currentLevel;

        [Inject]
        public void Construct(IProgressService progressService, GameStaticData gameStaticData)
        {
            _progressService = progressService;
            _gameStatic = gameStaticData;
        }

        private void Start()
        {
            InjectService.Instance.Inject(this);

            InstatiateLevel();
        }

        private void InstatiateLevel()
        {
            if (_currentLevel != null)
            {
                Destroy(_currentLevel);
            }

            var levelIndex = _progressService.PlayerProgress.WaveIndexProgress % _gameStatic.Parking.Parkings.Count;
            _currentLevel = Instantiate(_gameStatic.Parking.Parkings[levelIndex], Vector3.zero, quaternion.identity);
        }
    }
}