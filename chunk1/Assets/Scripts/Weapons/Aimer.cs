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
        public Action OnAimed;

        public float AimingSpeed = 180;

        public float Pitch;
        public float TargetPitch;
        public float TotalPitch { get { return (Pitch + _navigation.Pitch).Clamp360(); } }
        public bool IsAimed { get; private set; }
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
            _targeting.OnTargetChange += OnTargetChange;
        }

        public void Deinit()
        {
            _targeting.OnTargetChange -= OnTargetChange;
            Stop();
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
            UpdateTargetPitch();
            Pitch = GetPitchDelta(dt);
            _lastUpdateTime = _timeManager.GetTime();

            IsAimed = Mathf.Abs(Euler.Diff(TotalPitch, TargetPitch)) < 0.1f;
            if (IsAimed)
                CompleteAiming();
        }

        private void UpdateTargetPitch()
        {
            TargetPitch = _targeting.CurrentTarget != null
                ? CalculatePitch(_targeting.CurrentTarget.Navigation.Position)
                : _navigation.Pitch;
        }

        private void OnTargetChange(Unit target)
        {
            if (_update == null)
            {
                _timeManager.StartUpdate(ref _update, Update, 0.1f);
                if (OnAimingStarted != null)
                    OnAimingStarted();
                Update(0f);
            }
        }

        public float CalculatePitch(Vector3 target)
        {
            var direction = target - _navigation.Position;
            direction.y = 0;
            return Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
        }

        private void CompleteAiming()
        {
            if (_targeting.CurrentTarget == null)
            {
                _timeManager.StopUpdate(ref _update);
                if (OnAimingFinished != null)
                    OnAimingFinished();
            }
            else
            {
                if (OnAimed != null)
                    OnAimed();
            }
        }

        public void Stop()
        {
            _timeManager.StopUpdate(ref _update);
        }
    }
}
