using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Enums;
using Gameplay.Units;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Enemies.TowerStates
{
    public class ShooterBulletModel
    {
        public event Action Attacked;

        private ICoroutineService _coroutineService;
        private Unit _target;
        private float _duration;
        private Dictionary<EParameter, float> _parameters;

        public ShooterBulletModel(ICoroutineService coroutineService, Unit target,
            Dictionary<EParameter, float> parameters, float duration)
        {
            _coroutineService = coroutineService;
            _target = target;
            _parameters = parameters;
            _duration = duration;

            _coroutineService.StartCoroutine(DoDamage());
        }

        private IEnumerator DoDamage()
        {
            if (_target.IsDied) yield break;

            yield return new WaitForSeconds(_duration);
            _target.GetDamage(_parameters[EParameter.Damage]);

            Attacked?.Invoke();
        }
    }
}