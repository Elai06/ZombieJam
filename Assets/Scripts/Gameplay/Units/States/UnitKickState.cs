using DG.Tweening;
using Gameplay.Enums;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay.Units.States
{
    public class UnitKickState : UnitState
    {
        public UnitKickState(Unit unit) : base(EUnitState.Kick, unit)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _unit.transform.DOMoveY(4, 0.75f).SetEase(Ease.OutCubic);
            _unit.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                _unit.Died();
            });
        }
    }
}