using Assets.Scripts.Parts;
using UnityEngine;

namespace Assets.Scripts.Weapons
{
    public class GunView : PartView
    {
        public Transform Turret;

        private Gun _gun;
        private float _oldRotation;

        public override void Init(Part part)
        {
            base.Init(part);
            _gun = part as Gun;
        }

        void Update()
        {
            if (_gun.Aimer.Pitch != _oldRotation)
            {
                _oldRotation = _gun.Aimer.GetPitch(Time.time);
                Turret.rotation = Quaternion.Euler(0f, _oldRotation, 0f);
            }
        }
    }
}
