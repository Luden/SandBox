using System;
using Assets.Scripts.Movement;
using Assets.Scripts.Units;
using UnityEngine;
using Assets.Scripts.Core;

namespace Assets.Scripts.Weapons
{
    public class Aimer
    {
        public Action OnAimingStarted;
        public Action OnAimingFinished;

        public float AimingSpeed = 90;

        public float Pitch;
        public float TargetPitch;
        public float TotalPitch { get { return (Pitch + _navigation.Pitch).Clamp360(); } }
        public bool IsAimed { get { return Mathf.Abs(Euler.Diff(TotalPitch, TargetPitch)) < 0.1f; } }
        public bool IsAiming { get { return _update != null; } }

        private Navigation _navigation;
        private Targeting _targeting;
        private TimeManager _timeManager;
        private RegularUpdate _update;

        private float _lastUpdateTime;

        public Aimer(Navigation navigation, Targeting targeting, TimeManager timeManager)
        {
            _navigation = navigation;
            _timeManager = timeManager;
            _targeting = targeting;
        }

        public float GetPitch(float time)
        {
            if (IsAiming)
                return GetPitchDelta(time - _lastUpdateTime);
            else
                return Pitch;
        }

        private float GetPitchDelta(float dt)
        {
            var remaining = Euler.Diff(TotalPitch, TargetPitch);
            var sign = Math.Sign(remaining);
            var delta = Math.Min(AimingSpeed * dt, Mathf.Abs(remaining));
            return (Pitch + delta * sign).Clamp360();
        }

        private void Update(float dt)
        {
            Pitch = GetPitchDelta(dt);
            _lastUpdateTime = _timeManager.GetTime();

            if (IsAimed)
                CompleteAiming();
        }

        public bool Check()
        {
            if (_targeting.CurrentTarget != null)
                TargetPitch = CalculatePitch(_targeting.CurrentTarget.Navigation.Position);
            if (!IsAimed)
            {
                if (!IsAiming)
                    StartAiming(_targeting.CurrentTarget);
                return false;
            }
            return true;
        }

        public void StartAiming(Unit target)
        {
            if (!IsAimed)
            {
                _timeManager.StartUpdate(ref _update, Update, 0.1f);
                _lastUpdateTime = _timeManager.GetTime();
                if (OnAimingStarted != null)
                    OnAimingStarted();
            }
            else
                CompleteAiming();
        }

        public float CalculatePitch(Vector3 target)
        {
            var direction = target - _navigation.Position;
            direction.y = 0;
            return Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
        }

        private void CompleteAiming()
        {
            _timeManager.StopUpdate(ref _update);
            if (OnAimingFinished != null)
                OnAimingFinished();
        }

        public void Stop()
        {
            _timeManager.StopUpdate(ref _update);
        }
    }
}
