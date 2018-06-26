using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Movement
{
    public class Neighbourhood : MonoBehaviour
    {
        private NavMeshAgent _navMeshAgent;

        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        HashSet<Unit> _neighbours = new HashSet<Unit>();
        GameObject _reachedTarget = null;
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Unit")
                _neighbours.Add(other.gameObject.GetComponent<Unit>());
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Unit")
                _neighbours.Remove(other.gameObject.GetComponent<Unit>());
        }

        int _visitorHash = -1;
        List<Unit> _cacheNeighbours = new List<Unit>();
        List<Unit> GetNeighbours(bool isMoving)
        {
            _cacheNeighbours.Clear();
            foreach (var unit in _neighbours)
                if (unit.CommandProcessor.IsMoving() == isMoving)
                    _cacheNeighbours.Add(unit);
            return _cacheNeighbours;
        }

        bool IsUnitOrNeighboursReachedTarget(Vector3 point, int visitorHash)
        {
            if (visitorHash == _visitorHash)
                return false;

            _visitorHash = visitorHash;

            return IsUnitReachedTarget(point) || IsNeighboursReachedTarget(point, visitorHash);
        }

        public bool IsUnitReachedTarget(Vector3 point)
        {
            return (transform.position - point).sqrMagnitude < 1f;
        }

        public bool IsNeighboursReachedTarget(Vector3 point)
        {
            RebuildHash();
            return IsNeighboursReachedTarget(point, _visitorHash) && _neighbours.Count > 1;
        }

        bool IsNeighboursReachedTarget(Vector3 point, int visitorHash)
        {
            var staticNeighbours = GetNeighbours(false);
            if (staticNeighbours.Count == 0)
                return false;

            foreach (var unit in staticNeighbours)
                if (unit.Neighbourhood.IsUnitOrNeighboursReachedTarget(point, visitorHash))
                    return true;

            return false;
        }

        public Unit GetOppositeNeighbour()
        {
            var direction = _navMeshAgent.steeringTarget - transform.position;
            foreach (var unit in GetNeighbours(true))
            {
                var unitDirection = unit.NavMeshAgent.steeringTarget - unit.transform.position;
                if (Vector3.Dot(unitDirection, direction) < 0.1f)
                    return unit;
            }
            return null;
        }

        private void RebuildHash()
        {
            _visitorHash = Random.Range(0, int.MaxValue);
        }

        public static bool IsNeighbours(IEnumerable<Unit> units)
        {
            var anyUnit = units.FirstOrDefault();
            if (anyUnit == null)
                return false;

            anyUnit.Neighbourhood.RebuildHash();
            foreach (var unit in anyUnit.Neighbourhood._neighbours)
                unit.Neighbourhood.MarkNeighbours(anyUnit.Neighbourhood._visitorHash);

            foreach (var unit in units)
                if (unit.Neighbourhood._visitorHash != anyUnit.Neighbourhood._visitorHash)
                    return false;

            return true;
        }

        public void MarkNeighbours(int visitorHash)
        {
            if (visitorHash == _visitorHash)
                return;
            _visitorHash = visitorHash;
            foreach (var unit in _neighbours)
                unit.Neighbourhood.MarkNeighbours(visitorHash);
        }
    }
}
