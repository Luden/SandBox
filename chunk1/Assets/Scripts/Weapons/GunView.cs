using System.Collections;
using Assets.Scripts.Parts;
using UnityEngine;

namespace Assets.Scripts.Weapons
{
    public class GunView : PartView
    {
        public Transform Turret;

        private Gun _gun;
        private Coroutine _updateAiming;

        public override void Init(Part part)
        {
            base.Init(part);
            _gun = part as Gun;
            _gun.Aimer.OnAimingStarted += OnAimingStarted;
            _gun.Aimer.OnAimingFinished += OnAimingFinished;
        }

        private void OnAimingStarted()
        {
            _updateAiming = StartCoroutine(UpdateAiming());
        }

        private void OnAimingFinished()
        {
            StopCoroutine(_updateAiming);
        }

        private IEnumerator UpdateAiming()
        {
            while (true)
            {
                var rotation = _gun.Aimer.GetPitch(Time.time);
                Turret.localRotation = Quaternion.Euler(0f, rotation, 0f);
                yield return null;
            }
        }
    }
}
