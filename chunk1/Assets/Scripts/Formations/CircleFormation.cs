using System.Collections.Generic;
using Assets.Scripts.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Formations
{
    public class CircleFormation : FormationBase
    {
        protected int _reservedRings = 0;
        protected float SlotSize = 1.5f;

        public override FormationType GetKey()
        {
            return FormationType.Circle;
        }

        private void ReserveRing(int ring)
        {
            if (_reservedRings > ring)
                return;

            for (int i = _reservedRings; i <= ring; i++)
            {
                var radius = i * SlotSize;
                for (int j = 0; j < GetRingCount(i); j++)
                    ReserveItem(i, j, radius);
            }
            _reservedRings = ring + 1;
        }

        private int GetRingCount(int ring)
        {
            return ring == 0 ? 1 : ring * 6;
        }

        private void ReserveItem(int ring, int offsetInRing, float radius)
        {
            var OffsetInRingPart = ring == 0 ? 0f : (float)offsetInRing / GetRingCount(ring);
            var resultVector = new Vector3(Mathf.Cos(Mathf.PI * 2f * OffsetInRingPart) * radius, 0f, Mathf.Sin(Mathf.PI * 2f * OffsetInRingPart) * radius);
            _offsets.Add(resultVector);
        }
        
        NavMeshPath _path = new NavMeshPath();
        public override void Build()
        {
            if (_unitsCount == 0)
                return;

            _units.Clear();
            for (int i = 0; i < _unitsCount; i++)
                _units.Add(_unitStash[i]);

            var middle = GetMiddlePos(_unitStash, _unitsCount);
            CalculateAngles(middle);
            
            var lastRingCount = LastRingUnitsCount();
            if (lastRingCount > 0 && _unitsCount > 6)
            {
                _units.Sort(_deltaPosComparer);
                middle = GetMiddlePos(_units, _unitsCount - lastRingCount);
                CalculateAngles(middle);
            }

            _units.Sort(_deltaPosComparer);

            int ring = 0;
            int assignedCount = 0;
            int lastOffset = 0;
            while (assignedCount < _units.Count)
            {
                ReserveRing(ring);
                var ringCount = GetRingCount(ring);
                var slotsToReserve = Mathf.Min(_units.Count - assignedCount, ringCount);
                _angleComparer.SetRingOffset(ringCount);
                _units.Sort(assignedCount, slotsToReserve, _angleComparer);
                for (float i = lastOffset; (int)i < lastOffset + ringCount && assignedCount < _units.Count;)
                {
                    NavMeshHit hit;
                    var pos = _targetPosition + _offsets[(int)i];
                    var enabled = NavMesh.SamplePosition(pos, out hit, SlotSize / 2f, NavMesh.AllAreas);
                    if (enabled)
                    {
                        enabled = NavMesh.CalculatePath(_targetPosition, hit.position, NavMesh.AllAreas, _path);
                        if (enabled && (ring == 0 || GetPathLength(_path) < ring * SlotSize * 2))
                        {
                            _units[assignedCount].TargetPos = hit.position;
                            assignedCount++;
                            if (slotsToReserve < ringCount && _unitsCount > 3)
                            {
                                i += (float)ringCount / slotsToReserve;
                            }
                            else i++;
                        }
                        else i++;
                    }
                    else i++;
                }
                lastOffset += ringCount;
                ring++;

                if (lastOffset > _units.Count * 10 && ring > 10) // don't try too hard
                    break;
            }
        }

        private void CalculateAngles(Vector3 middle)
        {
            for (int i = 0; i < _unitsCount; i++)
            {
                var unit = _unitStash[i];
                unit.DeltaPos = unit.Pos - middle;
                unit.Angle = Pseudoangle(unit.DeltaPos.x, unit.DeltaPos.z);
                unit.DeltaPosSqrMagnitude = unit.DeltaPos.sqrMagnitude;
            }
        }

        private int LastRingUnitsCount()
        {
            int count = _unitsCount;
            int ring = 0;
            var ringUnitCount = GetRingCount(0);
            while (count > ringUnitCount)
            {
                count -= ringUnitCount;
                ring++;
                ringUnitCount = GetRingCount(ring);
            }
            return count;
        }
    }
}
