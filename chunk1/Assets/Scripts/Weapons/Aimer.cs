﻿using System;
using Assets.Scripts.Movement;
using Assets.Scripts.Units;
using UnityEngine;
using Assets.Scripts.Core;

namespace Assets.Scripts.Weapons
{
    public class Aimer
    {
        public Action OnAimingFinished;

        public float AimingSpeed = 5f;

        public float Pitch;
        public float TargetPitch;
        public float TotalPitch { get { return (Pitch + _navigation.Pitch).Clamp360(); } }
        public bool IsAimed { get { return Euler.Diff(Pitch, TargetPitch) > Mathf.Epsilon; } }
        public bool IsAiming { get { return _update != null; } }

        private Navigation _navigation;
        private Targeting _targeting;
        private TimeManager _timeManager;
        private RegularUpdate _update;

        public Aimer(Navigation navigation, Targeting targeting, TimeManager timeManager)
        {
            _navigation = navigation;
            _timeManager = timeManager;
            _targeting = targeting;
        }

        private void Update(float dt)
        {
            var remaining = Euler.Diff(TargetPitch, TotalPitch);
            var sign = Math.Sign(remaining);
            var delta = Math.Min(AimingSpeed * dt, Mathf.Abs(remaining));
            Pitch = (Pitch + delta * sign).Clamp360();

            if (!IsAimed)
                CompleteAiming();
        }

        public bool Check()
        {
            if (!IsAimed && !IsAiming)
            {
                StartAiming(_targeting.CurrentTarget);
                return false;
            }
            return true;
        }

        public void StartAiming(Unit target)
        {
            TargetPitch = CalculatePitch(target.Navigation.Position);
            if (IsAimed)
                _timeManager.StartUpdate(ref _update, Update, 0.1f);
            else
                CompleteAiming();
        }

        public float CalculatePitch(Vector3 target)
        {
            var direction = target - _navigation.Position;
            direction.y = 0;
            return Vector3.Angle(Vector3.forward, direction);
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