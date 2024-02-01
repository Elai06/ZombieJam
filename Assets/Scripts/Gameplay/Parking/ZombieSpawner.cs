using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Gameplay.Battle;
using Gameplay.Cards;
using Gameplay.CinemachineCamera;
using Gameplay.Configs.Zombies;
using Gameplay.Enums;
using Gameplay.Units;
using Gameplay.Windows.Gameplay;
using Infrastructure.StaticData;
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
        [SerializeField] private Transform _spawnPosition;
        [SerializeField] private TargetManager _targetManager;
        [SerializeField] private CameraSelector _cameraSelector;

        private readonly List<Unit> _zombies = new();

        private ICoroutineService _coroutineService;
        private IWindowService _windowService;
        private ICardsModel _cardsModel;
        private IGameplayModel _gameplayModel;

        private ZombieConfig _zombieConfig;

        [Inject]
        public void Construct(ICoroutineService coroutineService, IWindowService windowService,
            ICardsModel cardsModel, IGameplayModel gameplayModel, GameStaticData gameStaticData)
        {
            _coroutineService = coroutineService;
            _windowService = windowService;
            _cardsModel = cardsModel;
            _gameplayModel = gameplayModel;

            _zombieConfig = gameStaticData.ZombieConfig;
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
                unit.Kicked += OnKicked;
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
                var config = _zombieConfig.Config.Find(x => x.Name == spawnPosition.Name);
                var prefab = Instantiate(config.Prefab, spawnPosition.GetSpawnPosition(),
                    Quaternion.identity, _spawnPosition);
                prefab.transform.localPosition = spawnPosition.GetSpawnPosition();
                prefab.SetSwipeDirection(spawnPosition.GetSwipeDirection());
                prefab.Initialize(_cardsModel.CardModels[config.Name], _coroutineService, _targetManager, config);
                _zombies.Add(prefab);
            }
        }

        private void OnUnitDied(Unit unit)
        {
            if (Zombies.Any(x => !x.IsDied))
            {
                return;
            }

            _windowService.Open(WindowType.Died);
        }

        private void OnKicked(Unit unit)
        {
            unit.OnDied -= OnUnitDied;
            _zombies.Remove(unit);

            if (_gameplayModel.WaveType == EWaveType.Logic)
            {
                DOVirtual.DelayedCall(1, () => { _windowService.Open(WindowType.Died); });
            }
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