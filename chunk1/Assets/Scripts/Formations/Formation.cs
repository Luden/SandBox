using System.Collections.Generic;
using Assets.Scripts.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Formations
{
    public class FormationSlot
    {
        public Vector3 Pos;
        public Vector3 DeltaPos;
        public Vector3 TargetPos;
        public float DeltaPosSqrMagnitude;
        public float Angle;
        public int Index;
    }

    public class Formation : IKeyProvider<FormationType>
    {
        private List<FormationSlot> _unitStash = new List<FormationSlot>();
        private List<FormationSlot> _units = new List<FormationSlot>();
        private List<Vector3> _offsets = new List<Vector3>();
        private Vector3 _targetPosition;
        private int _unitsCount = 0;
        private int _reservedRings = 0;
        private DeltaPosComparer _deltaPosComparer = new DeltaPosComparer();
        private AngleComparer _angleComparer = new AngleComparer();

        public float SlotSize = 2f;

        private void ReserveRing(int ring)
        {
            if (_reservedRings > ring)
                return;

            int count = 1;
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

        public FormationType GetKey()
        {
            return FormationType.Movement;
        }

        public void Init(IEnumerable<Vector3> units, Vector3 pos)
        {
            int index = 0;
            _targetPosition = pos;

            NavMeshHit hit;
            var enabled = NavMesh.SamplePosition(pos, out hit, 50f, NavMesh.AllAreas);
            if (enabled)
                _targetPosition = hit.position;

            foreach (var unit in units)
            {
                if (_unitStash.Count <= index)
                    _unitStash.Add(new FormationSlot());
                _unitStash[index].Pos = unit;
                _unitStash[index].Index = index;
                _unitStash[index].TargetPos = _targetPosition;
                index++;
            }
            _unitsCount = index;

            if (enabled)
                Build();
        }

        NavMeshPath _path = new NavMeshPath();
        public void Build()
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

        private Vector3 GetMiddlePos(List<FormationSlot> units, int count)
        {
            var middle = Vector3.zero;
            if (count < 4)
            {
                var lowestXZ = units[0].Pos.x + units[0].Pos.z;
                middle = units[0].Pos;
                for (int i = 0; i < count; i++)
                {
                    var xz = units[i].Pos.x + units[i].Pos.z;
                    if (xz < lowestXZ)
                    {
                        lowestXZ = xz;
                        middle = units[i].Pos;
                    }
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                    middle += units[i].Pos;
                middle = middle / count;
            }
            return middle;
        }

        public Vector3 GetTargePos(int index)
        {
            return _unitStash[index].TargetPos;
        }

        public IEnumerable<FormationSlot> GetSlots()
        {
            for (int i = 0; i < _unitsCount; i++)
                yield return _unitStash[i];
        }

        public float GetPathLength(NavMeshPath path)
        {
            float lng = 0.0f;
            if (path.status != NavMeshPathStatus.PathInvalid && path.corners.Length > 1)
            {
                for (int i = 1; i < path.corners.Length; ++i)
                {
                    lng += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                }
            }

            return lng;
        }

        private float Pseudoangle(float dx, float dy)
        {
            var ax = Mathf.Abs(dx);
            var ay = Mathf.Abs(dy);
            var p = dy / (ax + ay);
            if (dx < 0)
                p = 2 - p;
            else if (dy < 0)
                p = 4 + p;
            return p;
        }

        class AngleComparer : IComparer<FormationSlot>
        {
            float _offset = 0f;
            const float _pseudoanglesPerRing = 4;

            public void SetRingOffset(int slotsPerRing)
            {
                _offset = (_pseudoanglesPerRing / (float)slotsPerRing) / 2f;
            }

            public int Compare(FormationSlot a, FormationSlot b)
            {
                var da = (_offset + a.Angle) % _pseudoanglesPerRing;
                var db = (_offset + b.Angle) % _pseudoanglesPerRing;
                return da.CompareTo(db);
            }
        }

        class DeltaPosComparer : IComparer<FormationSlot>
        {
            public int Compare(FormationSlot a, FormationSlot b)
            {
                return a.DeltaPosSqrMagnitude.CompareTo(b.DeltaPosSqrMagnitude);
            }
        }
    }
}
