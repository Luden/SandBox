using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Formations
{
    public class CroudFormation : FormationBase
    {
        public override FormationType GetKey()
        {
            return FormationType.Croud;
        }

        public override void Build()
        {
            if (_unitsCount == 0)
                return;

            _units.Clear();
            for (int i = 0; i < _unitsCount; i++)
                _units.Add(_unitStash[i]);

            var middle = GetMiddlePos(_unitStash, _unitsCount);
            CalculateDeltas(middle);
        }

        private void CalculateDeltas(Vector3 middle)
        {
            for (int i = 0; i < _unitsCount; i++)
            {
                var unit = _unitStash[i];
                unit.DeltaPos = unit.Pos - middle;
                unit.TargetPos = _targetPosition + unit.DeltaPos;
            }
        }
    }
}
