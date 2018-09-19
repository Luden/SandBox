using System;
using Assets.Scripts.Core;
using Assets.Scripts.Movement;
using Assets.Scripts.Units;

namespace Assets.Scripts.Weapons
{
    public class UnitTargeting
    {
        public Action<Unit> OnTargetChange;

        public Unit CurrentTarget { get; private set; }
        public Faction Faction { get; private set; }

        public UnitTargeting(Faction faction)
        {
            Faction = faction;
        }

        public void SetTarget(Unit target)
        {
            if (CurrentTarget != null)
                CurrentTarget.OnDeath -= OnDeath;
            CurrentTarget = target;
            if (CurrentTarget != null)
                CurrentTarget.OnDeath += OnDeath;
            CallTargetChange();
        }

        public bool IsTargetValid(Unit target)
        {
            return target.Player.Faction != Faction;
        }

        private void CallTargetChange()
        {
            if (OnTargetChange != null)
                OnTargetChange(CurrentTarget);
        }

        private void OnDeath(Unit unit)
        {
            SetTarget(null);
        }

        public void Stop()
        {
            if (CurrentTarget != null)
                CurrentTarget.OnDeath -= OnDeath;
            CurrentTarget = null;
            CallTargetChange();
        }
    }
}
