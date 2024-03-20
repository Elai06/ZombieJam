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

        private IEnemy _target;
        private float _duration;

        public ArcherBulletModel(ICoroutineService coroutineService, IEnemy target, float duration)
        {
            _target = target;
            _duration = duration;

            coroutineService.StartCoroutine(DoDamage());
        }

        private IEnumerator DoDamage()
        {
            if (_target.IsDied) yield break;

            yield return new WaitForSeconds(_duration);

            Attacked?.Invoke();
        }
    }
}