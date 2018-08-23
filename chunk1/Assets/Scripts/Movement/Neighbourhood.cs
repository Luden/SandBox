using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Assets.Scripts.Core;
using Assets.Scripts.Units;

namespace Assets.Scripts.Movement
{
    public class Neighbourhood
    {
        private IUnitObject _unitObject;
        private UnitManager _unitManager;

        public Neighbourhood(IUnitObject unitObject)
        {
            _unitObject = unitObject;
            _unitObject.OnColliderEnter += OnTriggerEnter;
        }

        HashSet<IUnitObject> _neighbours = new HashSet<IUnitObject>();
        GameObject _reachedTarget = null;
        void OnTriggerEnter(IUnitObject other)
        {
            _neighbours.Add(other);
        }

        void OnTriggerExit(IUnitObject other)
        {
            _neighbours.Remove(other);
        }

        int _visitorHash = -1;
        List<IUnitObject> _cacheNeighbours = new List<IUnitObject>();
        List<IUnitObject> GetNeighbours(bool isMoving)
        {
            _cacheNeighbours.Clear();
            foreach (var unit in _neighbours)
                if (unit.Owner.Navigation.IsMoving == isMoving)
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
                if (unit.Owner.Navigation.IsUnitOrNeighboursReachedTarget(point, _visitorHash))
                    return true;

            return false;
        }

        public int GetOppositeNeighbourPriority()
        {
            var direction = _unitObject.Owner.Navigation.SteeringTarget - _unitObject.Position;
            foreach (var unit in GetNeighbours(true))
            {
                var unitDirection = unit.Owner.Navigation.SteeringTarget - unit.Owner.Navigation.Position;
                if (Vector3.Dot(unitDirection, direction) < 0.1f)
                    return unit.NavMeshAgent.avoidancePriority;
            }
            return -1;
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
                unit.Owner.Navigation.Neighbourhood.MarkNeighbours(anyUnit.Navigation.Neighbourhood._visitorHash);

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
                unit.Owner.Navigation.Neighbourhood.MarkNeighbours(visitorHash);
        }
    }
}
