using System.Collections.Generic;
using Assets.Scripts.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Formations
{
    public class Slot
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
        private List<Slot> _unitStash = new List<Slot>();
        private List<Slot> _units = new List<Slot>();
        private List<Vector3> _offsets = new List<Vector3>();
        private Vector3 _targetPosition;
        private int _unitsCount = 0;
        private int _reservedRings = 0;
        private DeltaPosComparer _deltaPosComparer = new DeltaPosComparer();
        private AngleComparer _angleComparer = new AngleComparer();

        public float SlotSize = 1f;

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
                    _unitStash.Add(new Slot());
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
            if (_unitStash.Count == 0)
                return;

            var middle = Vector3.zero;
            foreach (var unit in _unitStash)
                middle += unit.Pos;
            middle = middle / _unitStash.Count;
            foreach (var unit in _unitStash)
            {
                unit.DeltaPos = unit.Pos - middle;
                unit.Angle = Pseudoangle(unit.DeltaPos.x, unit.DeltaPos.z);
                unit.DeltaPosSqrMagnitude = unit.DeltaPos.sqrMagnitude;
            }

            _units.Clear();

            for (int i = 0; i < _unitsCount; i++)
                _units.Add(_unitStash[i]);
            _units.Sort(_deltaPosComparer);

            int ring = 0;
            int assignedCount = 0;
            int lastOffset = 0;
            while (assignedCount < _units.Count)
            {
                ReserveRing(ring);
                var ringCount = GetRingCount(ring);
                var slotsToReserve = Mathf.Min(_units.Count - assignedCount, ringCount);
                _units.Sort(assignedCount, slotsToReserve, _angleComparer);
                for (int i = lastOffset; i < lastOffset + ringCount && assignedCount < _units.Count; i++)
                {
                    NavMeshHit hit;
                    var pos = _targetPosition + _offsets[i];
                    var enabled = NavMesh.SamplePosition(pos, out hit, SlotSize / 2f, NavMesh.AllAreas);
                    if (enabled)
                    {
                        enabled = NavMesh.CalculatePath(_targetPosition, hit.position, NavMesh.AllAreas, _path);
                        if (enabled && GetPathLength(_path) < ring * SlotSize * 2)
                        {
                            _units[assignedCount].TargetPos = hit.position;
                            assignedCount++;
                        }
                    }
                }
                lastOffset += ringCount;
                ring++;

                if (lastOffset > _units.Count * 10 && ring > 10) // don't try too hard
                    break;
            }
        }

        public Vector3 GetTargePos(int index)
        {
            return _unitStash[index].TargetPos;
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

        class AngleComparer : IComparer<Slot>
        {
            public int Compare(Slot a, Slot b)
            {
                return a.Angle.CompareTo(b.Angle);
            }
        }

        class DeltaPosComparer : IComparer<Slot>
        {
            public int Compare(Slot a, Slot b)
            {
                return a.DeltaPosSqrMagnitude.CompareTo(b.DeltaPosSqrMagnitude);
            }
        }
    }
}
