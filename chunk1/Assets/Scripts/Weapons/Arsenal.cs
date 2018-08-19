using System.Collections.Generic;
using Assets.Scripts.Movement;
using Assets.Scripts.Shots;
using Assets.Scripts.Units;

namespace Assets.Scripts.Weapons
{
    public class Arsenal
    {
        private TimeManager _timeManager;
        private ShotsManager _shotsManager;
        private Navigation _navigation;
        private RegularUpdate _update;
        private Targeting _targeting;
        private Following _following;

        public List<Gun> Guns = new List<Gun>();

        public Arsenal(Navigation navigation, Targeting targeting, Following following, ShotsManager shotsManager, TimeManager timeManager)
        {
            _shotsManager = shotsManager;
            _timeManager = timeManager;
            _targeting = targeting;
            _navigation = navigation;
            _following = following;

            Guns.Add(new Gun(_targeting, _navigation, _shotsManager, _timeManager));
            _targeting.OnTargetChange += OnTargetChange;
        }

        private void OnTargetChange(Unit unit)
        {
            if (unit == null)
                Stop();
            else
                _timeManager.StartUpdate(ref _update, Update, 0.1f);
        }

        private void Update(float dt)
        {
            if (_targeting.CurrentTarget == null)
            {
                Stop();
                return;
            }

            foreach (var gun in Guns)
                gun.CheckAiming();

            var isReachedTarget = IsReachedTarget();
            if (_following.CurrentTarget == _targeting.CurrentTarget && isReachedTarget == _following.IsActive)
                _following.Switch(!isReachedTarget);
        }

        public bool IsReachedTarget()
        {
            foreach (var gun in Guns)
                if (gun.CheckTarget(_targeting.CurrentTarget))
                    return true;
            return false;
        }

        public void Stop()
        {
            _timeManager.StopUpdate(ref _update);
        }
    }
}
