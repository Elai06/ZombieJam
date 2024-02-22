using System;
using Gameplay.Boosters;
using UnityEngine;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Effects
{
    public class BoosterEffectsManager : MonoBehaviour
    {
        [Inject] private IBoostersManager _boostersManager;

        [SerializeField] private ParticleSystem _relocationEffect;
        [SerializeField] private ParticleSystem _increaseAttackEffect;
        [SerializeField] private ParticleSystem _increaseAttackSpeedEffect;
        [SerializeField] private ParticleSystem _increaseHPEffect;
        [SerializeField] private ParticleSystem _increaseTravelSpeedEffect;

        private void Start()
        {
            InjectService.Instance.Inject(this);

            _boostersManager.Activate += OnActivate;
        }

        private void OnEnable()
        {
            if (_boostersManager != null)
            {
                _boostersManager.Activate += OnActivate;
            }
        }

        private void OnDisable()
        {
            _boostersManager.Activate -= OnActivate;
        }

        private void OnActivate(EBoosterType boosterType)
        {
            switch (boosterType)
            {
                case EBoosterType.Relocation:
                    _relocationEffect.Play();
                    break;
                case EBoosterType.IncreaseAttack:
                    _increaseAttackEffect.Play();
                    break;
                case EBoosterType.IncreaseAttackSpeed:
                    _increaseAttackSpeedEffect.Play();
                    break;
                case EBoosterType.IncreaseHP:
                    _increaseHPEffect.Play();
                    break;
                case EBoosterType.IncreaseTravelSpeed:
                    _increaseTravelSpeedEffect.Play();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(boosterType), boosterType, null);
            }
        }
    }
}