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
        private Navigation _navigation;
        private Faction _faction;

        public Targeting(Navigation navigation, Faction faction)
        {
            _navigation = navigation;
            _faction = faction;
        }

        public void SetTarget(Unit target)
        {
            CurrentTarget = target;
            CallTargetChange();
        }

        public bool IsTargetValid(Unit target)
        {
            return target.Player.Faction != _faction;
        }

        public bool CanAttack()
        {
            return false;
        }

        public bool IsTargetInRange()
        {
            return false;
        }

        private void CallTargetChange()
        {
            if (OnTargetChange != null)
                OnTargetChange(CurrentTarget);
        }

        public void Stop()
        {
            CurrentTarget = null;
            CallTargetChange();
        }
    }
}
