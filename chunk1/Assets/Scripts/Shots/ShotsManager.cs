using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Core;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.Shots
{
    public class ShotsManager : ManagerBase
    {
        public override ManagerType ManagerType { get { return ManagerType.Shots; } }

        private TimeManager _timeManager;
        private RegularUpdate _update;
        private List<Shot> _shots;

        public override void Init()
        {
            _timeManager = ManagerProvider.Instance.TimeManager;
            _timeManager.StartUpdate(ref _update, RegularUpdate, 0.1f);
        }

        public void AddShot(Shot shot)
        {
            _shots.Add(shot);
        }

        private void RegularUpdate(float dt)
        {
            for (int i = 0; i < _shots.Count; i++)
            {
                var shot = _shots[i];
                shot.Update(dt);

                var direction = shot.Position - shot.OldPosition;
                var magnitude = (shot.Position - shot.OldPosition).magnitude;
                RaycastHit hit;
                if (Physics.Raycast(shot.OldPosition, direction, out hit, magnitude))
                {
                    var unit = hit.transform.GetComponent<Unit>();
                    if (unit != null)
                        unit.Hull.ApplyShot(shot);

                    FastRemove(i--);
                }
            }
        }

        private void FastRemove(int index)
        {
            _shots[index] = _shots[_shots.Count - 1];
            _shots.RemoveAt(_shots.Count - 1);
        }
    }
}
