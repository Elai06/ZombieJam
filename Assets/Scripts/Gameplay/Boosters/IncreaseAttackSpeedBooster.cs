using Gameplay.Parking;

namespace Gameplay.Boosters
{
    public class IncreaseAttackSpeedBooster : Booster
    {
        private ZombieSpawner _zombieSpawner;

        private bool _isActive;

        protected override void Start()
        {
            _boosterType = EBoosterType.IncreaseAttackSpeed;

            base.Start();

            _zombieSpawner = FindObjectOfType<ZombieSpawner>();
        }

        protected override void Activate()
        {
            if (_isActive) return;
            _isActive = true;

            foreach (var zombie in _zombieSpawner.Zombies)
            {
                var parameters = zombie.Parameters;
                var increaseValue = parameters[_parameter.ParameterType] * _parameter.IncreaseValue;
                parameters[_parameter.ParameterType] += increaseValue;
            }

            _boostersManager.ConsumeBooster(_boosterType, 1);
        }
    }
}