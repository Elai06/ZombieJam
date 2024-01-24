using System.Linq;
using Gameplay.Enums;
using Gameplay.Parking;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Boosters
{
    public class RelocationBooster : Booster
    {
        [SerializeField] private ZombieSpawner _zombieSpawner;

        protected override void Start()
        {
            base.Start();

            _boosterType = EBoosterType.Relocation;
            _zombieSpawner = FindObjectOfType<ZombieSpawner>();
        }

        protected override void Activate()
        {
            if (_zombieSpawner == null)
            {
                _zombieSpawner = FindObjectOfType<ZombieSpawner>();
            }

            var zombiesInParking = _zombieSpawner.Zombies.Where(x => x.CurrentState == EUnitState.Parking).ToList();

            if (zombiesInParking.Count < 2)
            {
                Debug.LogError("Для обмена местами необходимо указать как минимум 2 объекта.");
                return;
            }

            // Перебираем объекты в первой половине массива
            for (int i = 0; i < zombiesInParking.Count; i++)
            {
                // Генерируем случайный индекс из диапазона, начиная от текущего индекса до конца массива
                var currentUnit = zombiesInParking[i];
                var zombies = zombiesInParking.Where(x => x.Config.ZombieSize == currentUnit.Config.ZombieSize).ToList();
                if (zombies.Count <= 1)
                {
                    Debug.Log($"{currentUnit.UnitClass} {zombies.Count}");
                    continue;
                }

                var randomIndex = Random.Range(0, zombies.Count);
                var randomUnit = zombies[randomIndex];

                while (currentUnit == randomUnit)
                {
                    randomIndex = Random.Range(0, zombies.Count);
                    randomUnit = zombies[randomIndex];
                }

                // Запоминаем текущую позицию текущего объекта
                Vector3 currentPosition = currentUnit.transform.position;

                var currentSwipeDirection = currentUnit.SwipeDirection;
                var randomSwipeDirection = randomUnit.SwipeDirection;

                // Меняем позиции местами между текущим объектом и случайно выбранным объектом
                zombiesInParking[i].transform.position = randomUnit.transform.position;
                zombies[randomIndex].transform.position = currentPosition;

                //Устанавливаем вектор направления
                currentUnit.SetSwipeDirection(randomSwipeDirection);
                randomUnit.SetSwipeDirection(currentSwipeDirection);
                currentUnit.ResetMovingAfterBooster();
                randomUnit.ResetMovingAfterBooster();
            }

            _boostersManager.ConsumeBooster(_boosterType, 1);
        }
    }
}