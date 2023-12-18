using System.Collections.Generic;
using Gameplay.Battle;
using Gameplay.Configs;
using Infrastructure.UnityBehaviours;
using UnityEngine;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Enemies
{
    public class EnemyInitializer : MonoBehaviour
    {
        [SerializeField] private List<Enemy> _enemies;
        [SerializeField] private BuildingConfig _config;
        [SerializeField] private TargetManager _targetManager;

      [Inject]  private ICoroutineService _coroutineService;

      public List<Enemy> Enemies => _enemies;

      private void Start()
        {
            InjectService.Instance.Inject(this);
            
            foreach (var enemy in _enemies)
            {
                var configData = _config.GetBuildingConfig(enemy.BuildingType).Parameters;
                enemy.Initialize(configData, _coroutineService, _targetManager);
            }
        }
    }
}