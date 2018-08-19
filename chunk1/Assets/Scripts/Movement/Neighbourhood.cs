using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Assets.Scripts.Core;
using Assets.Scripts.Units;

namespace Assets.Scripts.Movement
{
    public class Neighbourhood : MonoBehaviour
    {
        private NavMeshAgent _navMeshAgent;

        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        HashSet<Navigation> _neighbours = new HashSet<Navigation>();
        GameObject _reachedTarget = null;
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == Tags.Unit)
                _neighbours.Add(other.gameObject.GetComponent<Navigation>());
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == Tags.Unit)
                _neighbours.Remove(other.gameObject.GetComponent<Navigation>());
        }

        int _visitorHash = -1;
        List<Navigation> _cacheNeighbours = new List<Navigation>();
        List<Navigation> GetNeighbours(bool isMoving)
        {
            _cacheNeighbours.Clear();
            foreach (var unit in _neighbours)
                if (unit.IsMoving == isMoving)
                    _cacheNeighbours.Add(unit);
            return _cacheNeighbours;
        }

        public bool IsNeighboursReachedTarget(Vector3 point, int visitorHash)
        {
            if (visitorHash == -1)
                RebuildHash();
            else if (visitorHash == _visitorHash)
                return false;
            else
                _visitorHash = visitorHash;

            var staticNeighbours = GetNeighbours(false);
            if (staticNeighbours.Count == 0)
                return false;

            foreach (var unit in staticNeighbours)
                if (unit.IsUnitOrNeighboursReachedTarget(point, _visitorHash))
                    return true;

            return false;
        }

        public Navigation GetOppositeNeighbour()
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

            anyUnit.Navigation.Neighbourhood.RebuildHash();
            foreach (var unit in anyUnit.Navigation.Neighbourhood._neighbours)
                unit.Neighbourhood.MarkNeighbours(anyUnit.Navigation.Neighbourhood._visitorHash);

            foreach (var unit in units)
                if (unit.Navigation.Neighbourhood._visitorHash != anyUnit.Navigation.Neighbourhood._visitorHash)
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
