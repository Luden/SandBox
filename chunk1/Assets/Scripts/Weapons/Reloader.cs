using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Weapons
{
    public class Reloader
    {
        public Action OnReloadingFinish;

        public float ReloadTime = 5f;
        public float ReloadTimeDispersion = 0.1f;

        public bool IsReloading { get { return ReloadRemainingTime > 0f; } }
        public float ReloadRemainingTime;

        private TimeManager _timeManager;
        private RegularUpdate _update;

        public Reloader(TimeManager timeManager)
        {
            _timeManager = timeManager;
        }

        private void Update(float dt)
        {
            ReloadRemainingTime -= dt;
            if (!IsReloading)
                CompleteReloading();
        }

        public void StartReloading()
        {
            ReloadRemainingTime = ReloadTime + ReloadTime * UnityEngine.Random.Range(-ReloadTimeDispersion / 2f, ReloadTimeDispersion / 2f);
            _timeManager.StartUpdate(ref _update, Update, 0.1f);
        }

        private void CompleteReloading()
        {
            _timeManager.StopUpdate(ref _update);
            ReloadRemainingTime = 0f;
            if (OnReloadingFinish != null)
                OnReloadingFinish();
        }

        public void Stop()
        {
            _timeManager.StopUpdate(ref _update);
        }
    }
}
