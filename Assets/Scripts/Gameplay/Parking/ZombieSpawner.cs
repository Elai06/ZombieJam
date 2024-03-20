using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Gameplay.Battle;
using Gameplay.Cards;
using Gameplay.CinemachineCamera;
using Gameplay.Configs.Zombies;
using Gameplay.Enums;
using Gameplay.Units;
using Gameplay.Windows;
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
            _gameplayModel.InitializeZombieSpawner(this);

            foreach (var unit in Zombies)
            {
                unit.OnDied += OnUnitDied;
                unit.Kicked += OnKicked;
                unit.DoDamage += OnUnitDoDamage;
                unit.StateMachine.OnStateChange += OnZombieStateChanged;
            }

            _gameplayModel.OnRevive += ReviveUnits;
        }

        private void OnDisable()
        {
            _gameplayModel.OnRevive -= ReviveUnits;
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
                prefab.SetSwipeDirection(GetSwipeDirection(spawnPosition.SwipeSide), spawnPosition.SwipeSide);
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
            unit.DoDamage -= OnUnitDoDamage;
            unit.OnDied -= OnUnitDied;
            unit.Kicked -= OnKicked;
            unit.DoDamage -= OnUnitDoDamage;
            unit.StateMachine.OnStateChange -= OnZombieStateChanged;

            if (_gameplayModel.WaveType == EWaveType.Logic)
            {
                _windowService.Open(WindowType.Died);
                _windowService.Close(WindowType.Died);

                var diedView = _windowService.CashedWindows[WindowType.Died].GetComponent<DiedView>();
                diedView.DiedFromPatrol = true;
                
                DOVirtual.DelayedCall(1, () =>
                {
                    _windowService.Open(WindowType.Died);
                });
            }
        }


        private void OnZombieStateChanged()
        {
            if (_zombies.Any(zombie => zombie.CurrentState == EUnitState.Parking 
                                       || zombie.CurrentState == EUnitState.Road))
            {
                return;
            }

            _cameraSelector.ChangeCamera(ECameraType.Enemies);
        }

        private void ReviveUnits()
        {
            foreach (var zombie in _zombies)
            {
                zombie.Revive();
            }

            _windowService.Open(WindowType.Gameplay);
            _windowService.Close(WindowType.Died);
        }

        private void OnUnitDoDamage()
        {
            if (!_gameplayModel.IsWasFirstDamage && this != null)
            {
                _gameplayModel.FirstDamage();
            }
        }

        private ESwipeDirection GetSwipeDirection(ESwipeSide eSwipeSide)
        {
            if (eSwipeSide == ESwipeSide.Back || eSwipeSide == ESwipeSide.Forward)
            {
                return ESwipeDirection.Vertical;
            }

            return ESwipeDirection.Horizontal;
        }
    }
}