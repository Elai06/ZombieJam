using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Battle;
using Gameplay.Cards;
using Gameplay.CinemachineCamera;
using Gameplay.Configs;
using Gameplay.Enums;
using Gameplay.Units;
using Gameplay.Windows.Gameplay;
using Infrastructure.UnityBehaviours;
using Infrastructure.Windows;
using UnityEngine;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Parking
{
    public class ZombieSpawner : MonoBehaviour
    {
        [SerializeField] private PositionSpawner _positionSpawner;
        [SerializeField] private ZombieConfig _zombieConfig;
        [SerializeField] private Transform _spawnPosition;
        [SerializeField] private TargetManager _targetManager;
        [SerializeField] private CameraSelector _cameraSelector;

        private readonly List<Unit> _zombies = new();

        private ICoroutineService _coroutineService;
        private IWindowService _windowService;
        private ICardsModel _cardsModel;
        private IGameplayModel _gameplayModel;

        [Inject]
        public void Construct(ICoroutineService coroutineService, IWindowService windowService,
            ICardsModel cardsModel, IGameplayModel gameplayModel)
        {
            _coroutineService = coroutineService;
            _windowService = windowService;
            _cardsModel = cardsModel;
            _gameplayModel = gameplayModel;
        }

        public List<Unit> Zombies => _zombies;

        private void Start()
        {
            InjectService.Instance.Inject(this);

            Initialize();
        }

        private void Initialize()
        {
            Spawn();

            foreach (var unit in Zombies)
            {
                unit.OnDied += OnUnitDied;
                unit.StateMachine.OnStateChange += OnZombieStateChanged;
            }

            _gameplayModel.OnResurection += ResurectionUnits;
        }

        private void OnDisable()
        {
            _gameplayModel.OnResurection -= ResurectionUnits;
        }

        private void Spawn()
        {
            var positions = _positionSpawner.GetSpawnPositions();

            foreach (var spawnPosition in positions)
            {
                if (!spawnPosition.IsAvailablePosition()) continue;
                var config = _zombieConfig.Config.Find(x => x.Type == spawnPosition.unitClass);
                var prefab = Instantiate(config.Prefab, spawnPosition.GetSpawnPosition(),
                    Quaternion.identity, _spawnPosition);
                prefab.transform.localPosition = spawnPosition.GetSpawnPosition();
                prefab.SetSwipeDirection(spawnPosition.GetSwipeDirection());
                prefab.Initialize(_cardsModel.CardModels[config.Type], _coroutineService, _targetManager,
                    config.Type);
                _zombies.Add(prefab);
            }
        }

        private void OnUnitDied()
        {
            if (Zombies.Any(unit => !unit.IsDied))
            {
                return;
            }

            _windowService.Open(WindowType.Died);
        }

        private void OnZombieStateChanged()
        {
            if (_zombies.Any(zombie => zombie.CurrentState != EUnitState.Battle))
            {
                return;
            }

            _cameraSelector.ChangeCamera(ECameraType.Enemies);
        }

        private void ResurectionUnits()
        {
            foreach (var zombie in _zombies)
            {
                zombie.Resurection();
            }

            _windowService.Open(WindowType.Gameplay);
            _windowService.Close(WindowType.Died);
        }
    }
}