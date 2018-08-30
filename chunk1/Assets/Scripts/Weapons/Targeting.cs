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
        private Faction _faction;

        public Targeting(Navigation navigation, Faction faction)
        {
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
