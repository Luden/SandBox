using Assets.Scripts.Parts;
using Assets.Scripts.Movement;
using Assets.Scripts.Shots;
using Assets.Scripts.Units;
using UnityEngine;
using System;

namespace Assets.Scripts.Weapons
{
    public class Gun : Part
    {
        public Vector3 BarrelOffset;
        public override PartType Type { get { return PartType.None; } }
        public float Range = 10f;

        private TimeManager _timeManager;
        private Navigation _navigation;
        private Targeting _targeting;
        private ShotsManager _shotsManager;
        private Vector3 _targetPosition;

        public Reloader Reloader { get; private set; }
        public Shooter Shooter { get; private set; }
        public Aimer Aimer { get; private set; }

        public void Init(Targeting targeting, Navigation navigation, ShotsManager shotsManager, TimeManager timeManager)
        {
            _navigation = navigation;
            _targeting = targeting;
            _timeManager = timeManager;
            _shotsManager = shotsManager;
            Reloader = new Reloader(timeManager);
            Shooter = new Shooter(timeManager);
            Aimer = new Aimer(_navigation, _targeting, _timeManager);

            Reloader.OnReloadingFinish += OnReloadingFinish;
            Shooter.OnShootingFinished += OnShootingFinished;
            Aimer.OnAimingFinished += OnAimingFinished;
            Shooter.OnShoot += OnShoot;
        }

        private void OnReloadingFinish()
        {
            if (!IsTargetValid())
                return;
            if (Aimer.Check())
                StartShooting();
        }

        private void OnShootingFinished()
        {
            Reloader.StartReloading();

            if (!IsTargetValid())
                return;

            Aimer.Check();
        }

        public void CheckAiming()
        {
            Aimer.Check();
        }

        private void OnAimingFinished()
        {
            if (!IsTargetValid())
                return;

            if (!Reloader.IsReloading)
                StartShooting();
        }

        private void OnShoot()
        {
            var worldOffset = Quaternion.AngleAxis(Aimer.Pitch, Vector3.up) * BarrelOffset;
            _shotsManager.AddShot(_navigation.Position + worldOffset, _targetPosition);
        }

        public void StartShooting()
        {
            if (!Shooter.IsShooting)
            {
                _targetPosition = _targeting.CurrentTarget.Navigation.Position;
                Shooter.StartShooting();
            }
        }

        public bool CanShootNow()
        {
            if (!CheckTarget(_targeting.CurrentTarget))
                return false;

            return !Shooter.IsShooting && !Reloader.IsReloading && Aimer.IsAimed;
        }

        private bool IsTargetValid()
        {
            return CheckTarget(_targeting.CurrentTarget);
        }

        public bool CheckTarget(Unit target)
        {
            if (target == null)
                return false;
            if (target.Hull.IsDead)
                return false;
            if ((_navigation.Position - target.Navigation.Position).magnitude > Range)
                return false;

            return true;
        }

        public void Stop()
        {
            Reloader.Stop();
            Aimer.Stop();
            Shooter.Stop();
        }
    }
}
