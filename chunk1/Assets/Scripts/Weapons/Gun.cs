using Assets.Scripts.Movement;
using Assets.Scripts.Shots;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.Weapons
{
    public class Gun
    {
        public float Range = 10f;

        private TimeManager _timeManager;
        private Navigation _navigation;
        private Targeting _targeting;
        private Reloader _reloader;
        private Shooter _shooter;
        private Aimer _aimer;
        private ShotsManager _shotsManager;
        private Vector3 _targetPosition;

        public Gun(Targeting targeting, Navigation navigation, ShotsManager shotsManager, TimeManager timeManager)
        {
            _navigation = navigation;
            _targeting = targeting;
            _timeManager = timeManager;
            _shotsManager = shotsManager;
            _reloader = new Reloader(timeManager);
            _shooter = new Shooter(timeManager);
            _aimer = new Aimer(_navigation, _targeting, _timeManager);

            _reloader.OnReloadingFinish += OnReloadingFinish;
            _shooter.OnShootingFinished += OnShootingFinished;
            _aimer.OnAimingFinished += OnAimingFinished;
            _shooter.OnShoot += OnShoot;
        }

        private void OnReloadingFinish()
        {
            if (!IsTargetValid())
                return;
            if (_aimer.Check())
                StartShooting();
        }

        private void OnShootingFinished()
        {
            _reloader.StartReloading();

            if (!IsTargetValid())
                return;

            _aimer.Check();
        }

        public void CheckAiming()
        {
            _aimer.Check();
        }

        private void OnAimingFinished()
        {
            if (!IsTargetValid())
                return;

            if (!_reloader.IsReloading)
                StartShooting();
        }

        private void OnShoot()
        {
            _shotsManager.AddShot(new Shot(_navigation.Position, _targetPosition));
        }

        public void StartShooting()
        {
            if (!_shooter.IsShooting)
            {
                _targetPosition = _targeting.CurrentTarget.Navigation.Position;
                _shooter.StartShooting();
            }
        }

        public bool CanShootNow()
        {
            if (!CheckTarget(_targeting.CurrentTarget))
                return false;

            return !_shooter.IsShooting && !_reloader.IsReloading && !_aimer.IsAimed;
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
    }
}
