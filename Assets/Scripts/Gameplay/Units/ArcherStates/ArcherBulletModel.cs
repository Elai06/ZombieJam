using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Enemies;
using Gameplay.Enums;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Units.ArcherStates
{
    public class ArcherBulletModel
    {
        public event Action Attacked;

        private ICoroutineService _coroutineService;
        private IEnemy _target;
        private float _duration;
        private Dictionary<EParameter, float> _parameters;

        public ArcherBulletModel(ICoroutineService coroutineService, IEnemy target,
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
            _target.GetDamage(_parameters[EParameter.Damage], false);

            Attacked?.Invoke();
        }
    }
}