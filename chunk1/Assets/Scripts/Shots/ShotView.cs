using System.Collections.Generic;
using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.Shots
{
    public class ShotView : MonoBehaviour
    {
        private Shot _shot;

        public void Init(Shot shot)
        {
            _shot = shot;
        }

        private void Update()
        {
            transform.position = _shot.GetPosition(Time.time);
        }
    }
}
