using System;
using Assets.Scripts.Core;
using Assets.Scripts.Movement;
using Assets.Scripts.Units;

namespace Assets.Scripts.Weapons
{
    public class Targeting
    {
        public Action<Unit> OnTargetChange;

        public Unit CurrentTarget { get; private set; }

        private UnitTargeting _unitTargeting;

        public Targeting(UnitTargeting unitTargeting)
        {
            _unitTargeting = unitTargeting;
            _unitTargeting.OnTargetChange += OnUnitTargetChange;
        }

        public bool IsUnitTarget()
        {
            return _unitTargeting.CurrentTarget != null
                && _unitTargeting.CurrentTarget == CurrentTarget;
        }

        private void OnUnitTargetChange(Unit target)
        {
            SetTarget(target);
        }

        public void SetTarget(Unit target)
        {
            if (CurrentTarget != null)
                CurrentTarget.OnDeath -= OnTargetDeath;
            CurrentTarget = target;
            if (CurrentTarget != null)
                CurrentTarget.OnDeath += OnTargetDeath;
            CallTargetChange();
        }

        private void CallTargetChange()
        {
            if (OnTargetChange != null)
                OnTargetChange(CurrentTarget);
        }

        private void OnTargetDeath(Unit unit)
        {
            SetTarget(null);
        }

        public void Deinit()
        {
            if (CurrentTarget != null)
                CurrentTarget.OnDeath -= OnTargetDeath;
            CurrentTarget = null;
            _unitTargeting.OnTargetChange -= OnUnitTargetChange;
        }
    }
}
