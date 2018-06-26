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

    public class AngleComparer : IComparer<FormationSlot>
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

    public class DeltaPosComparer : IComparer<FormationSlot>
    {
        public int Compare(FormationSlot a, FormationSlot b)
        {
            return a.DeltaPosSqrMagnitude.CompareTo(b.DeltaPosSqrMagnitude);
        }
    }

    public class FormationBase : IKeyProvider<FormationType>
    {
        protected List<FormationSlot> _unitStash = new List<FormationSlot>();
        protected List<FormationSlot> _units = new List<FormationSlot>();
        protected List<Vector3> _offsets = new List<Vector3>();
        protected Vector3 _targetPosition;
        protected int _unitsCount = 0;
        protected DeltaPosComparer _deltaPosComparer = new DeltaPosComparer();
        protected AngleComparer _angleComparer = new AngleComparer();

        public virtual FormationType GetKey()
        {
            return FormationType.None;
        }

        public virtual void Init(IEnumerable<Vector3> units, Vector3 pos)
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

        public virtual void Build()
        {
        }

        protected Vector3 GetMiddlePos(List<FormationSlot> units, int count)
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

        protected static float GetPathLength(NavMeshPath path)
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

        protected static float Pseudoangle(float dx, float dy)
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
    }
}
