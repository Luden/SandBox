using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Shots;

namespace Assets.Scripts.Units
{
    public class Hull
    {
        public float Health;
        public bool IsDead { get { return Health <= 0f; } }

        public Action OnDeath;
        public Action OnDamage;

        public Hull()
        {
            Health = 100f;
        }

        public void ApplyShot(Shot shot)
        {
            Health -= shot.Damage;
            if (OnDamage != null)
                OnDamage();

            if (Health < 0f)
                Die();
        }

        public void Die()
        {
            if (OnDeath != null)
                OnDeath();
        }
    }
}
