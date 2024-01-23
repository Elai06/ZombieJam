using Gameplay.Configs.Boosters;
using Gameplay.Parameters;
using Infrastructure.StaticData;
using UnityEngine;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Boosters
{
    public abstract class Booster : MonoBehaviour
    {
        [Inject] protected IBoostersManager _boostersManager;
        [Inject] protected GameStaticData _gameStaticData;

        protected BoosterConfigData _parameter;

        protected EBoosterType _boosterType;

        protected virtual void Start()
        {
            InjectService.Instance.Inject(this);
            _parameter = _gameStaticData.BoostersConfig.GetBoosterConfig(_boosterType);

            _boostersManager.Activate += OnActivate;
        }

        private void OnDisable()
        {
            _boostersManager.Activate -= OnActivate;
        }

        private void OnActivate(EBoosterType boosterType)
        {
            if (_boosterType == boosterType)
            {
                Activate();
            }
        }

        protected virtual void Activate()
        {
        }
    }
}