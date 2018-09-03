using System;

namespace Assets.Scripts.Weapons
{
    public class Shooter
    {
        public Action OnShoot;
        public Action OnShootingFinished;

        public float BarrageDelay = 0.5f;
        public int ShotsInBarrage = 3;

        public float ShootCooldown;
        public bool IsShooting { get { return _update != null; } }
        public int RemainingShots;

        private TimeManager _timeManager;
        private RegularUpdate _update;

        public Shooter(TimeManager timeManager)
        {
            _timeManager = timeManager;
        }

        private void Update(float dt)
        {
            ShootCooldown -= dt;
            if (ShootCooldown > 0)
                return;

            MakeShot();

            if (RemainingShots <= 0)
                FinishShooting();
        }

        public void StartShooting()
        {
            ShootCooldown = 0f;
            RemainingShots = ShotsInBarrage;
            _timeManager.StartUpdate(ref _update, Update, 0.1f);
        }

        private void MakeShot()
        {
            RemainingShots--;
            ShootCooldown = BarrageDelay;
            if (OnShoot != null)
                OnShoot();
        }

        private void FinishShooting()
        {
            _timeManager.StopUpdate(ref _update);
            if (OnShootingFinished != null)
                OnShootingFinished();
        }

        public void Stop()
        {
            _timeManager.StopUpdate(ref _update);
        }
    }
}
