using Assets.Scripts.Units;
using Assets.Scripts.Weapons;
using UnityEngine;

namespace Assets.Scripts.Movement
{
    public class Following
    {
        private readonly float _minDelta = Mathf.Epsilon;

        private Unit _target;
        public Unit CurrentTarget { get; private set; }
        private TimeManager _timeManager;
        private Navigation _navigation;
        private RegularUpdate _update;

        private Vector3 _lastTargetPos;

        public bool IsActive { get { return _update != null; } }

        public Following(Navigation navigation, TimeManager timeManager)
        {
            _timeManager = timeManager;
            _navigation = navigation;
        }

        public void SetTarget(Unit target)
        {
            if (!IsTartgetValid(target))
                return;

            _target = target;
            CurrentTarget = _target;
            _lastTargetPos = Vector3.zero;
            _timeManager.StartUpdate(ref _update, Update, 1f);
            Update(0f);
        }

        public void Stop()
        {
            _timeManager.StopUpdate(ref _update);
            _target = null;
            CurrentTarget = null;
        }

        public void Switch(bool active)
        {
            if (active)
                Unpause();
            else
                Pause();
        }

        public void Pause()
        {
            if (!IsActive)
                return;

            _timeManager.StopUpdate(ref _update);
            _navigation.Stop();
            _target = null;
        }

        public void Unpause()
        {
            if (!IsTartgetValid(CurrentTarget))
                return;

            if (IsActive)
                return;

            SetTarget(CurrentTarget);
        }

        private void Update(float dt)
        {
            if (!IsTartgetValid(_target))
                Stop();

            var targetPos = _target.Navigation.Position;
            if (Vector3.SqrMagnitude(targetPos - _lastTargetPos) < 0.1f)
                return;

            _lastTargetPos = targetPos;
            _navigation.Go(_lastTargetPos);
        }

        private bool IsTartgetValid(Unit target)
        {
            return target != null && !target.Hull.IsDead;
        }
    }
}
