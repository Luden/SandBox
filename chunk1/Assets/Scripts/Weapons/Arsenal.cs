using System.Collections.Generic;
using Assets.Scripts.Movement;
using Assets.Scripts.Parts;
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
        private Partset _partset;
        private int _ownerId;

        public List<Gun> Guns = new List<Gun>();

        public Arsenal(int ownerId, Navigation navigation, Targeting targeting, Following following, Partset partset, ShotsManager shotsManager, TimeManager timeManager)
        {
            _ownerId = ownerId;
            _shotsManager = shotsManager;
            _timeManager = timeManager;
            _targeting = targeting;
            _navigation = navigation;
            _following = following;
            _partset = partset;

            _partset.OnPartAttached += OnPartAttached;
            _partset.OnPartDetached += OnPartDetached;

            _targeting.OnTargetChange += OnTargetChange;
        }

        private void OnPartAttached(Slot slot)
        {
            if (slot.Part.Type != PartType.Gun)
                return;

            var gun = slot.Part as Gun;
            if (gun == null || Guns.Contains(gun))
                return;

            gun.Init(_ownerId, slot.Offset, _targeting, _navigation, _shotsManager, _timeManager);
            Guns.Add(gun);
        }

        private void OnPartDetached(Part part, int slot)
        {
            if (part.Type != PartType.Gun)
                return;

            var gun = part as Gun;
            if (gun == null || !Guns.Contains(gun))
                return;

            gun.Deinit();
            Guns.Remove(gun);
        }

        private void OnTargetChange(Unit unit)
        {
            if (unit == null)
                Stop();
            else
            {
                _timeManager.StartUpdate(ref _update, Update, 0.1f);
                Update(0);
            }
        }

        private void Update(float dt)
        {
            if (_targeting.CurrentTarget == null)
            {
                Stop();
                return;
            }

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
            foreach (var gun in Guns)
                gun.Stop();
        }
    }
}
