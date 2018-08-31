using System;
using Assets.Scripts;
using Assets.Scripts.Commands;
using Assets.Scripts.Movement;
using Assets.Scripts.Units;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Movement
{
    public class Navigation
    {
        public Neighbourhood Neighbourhood;
        public Action OnCancel;
        public Action OnFinish;

        public Vector3 Position { get { return _unitObject.Position; } }
        public float Pitch { get { return _unitObject.Rotation.eulerAngles.y; } }
        public Vector3 SteeringTarget { get { return _unitObject.NavMeshAgent.steeringTarget; } }
        
        private const int StopPriority = 50;
        private const int MovePriority = 48;
        private const int GiveWayPriority = 49;
        private const float StopCheckSpeedPart = 0.1f;
        private const float StopTime = 1f;

        private IUnitObject _unitObject;
        private RegularUpdate _update;
        private TimeManager _timeManager;
        private Vector3 _initialTarget;
        private Vector3 _navMeshTarget;

        private bool _alreadyReachedTarget = false;
        private float _lastDistanceToTarget = 0f;

        public Navigation(IUnitObject unitObject)
        {
            var provider = ManagerProvider.Instance;
            _timeManager = provider.TimeManager;
            _unitObject = unitObject;
            Neighbourhood = new Neighbourhood(_unitObject);
        }

        public void Go(Vector3 target)
        {
            _alreadyReachedTarget = false;
            _navMeshTarget = _initialTarget = target;
            IsMoving = true;

            NavMeshHit hit;
            if (!NavMesh.SamplePosition(_initialTarget, out hit, 100f, NavMesh.AllAreas))
            {
                Cancel();
                return;
            }

            _navMeshTarget = hit.position;

            _unitObject.NavMeshAgent.SetDestination(_navMeshTarget);
            _unitObject.NavMeshAgent.avoidancePriority = MovePriority;
            _timeManager.StartUpdate(ref _update, RegularUpdate, 0.1f);
        }

        public bool IsMoving { get; private set; }

        private void Cancel()
        {
            Stop();
            if (OnCancel != null)
                OnCancel();
        }

        private void Finish()
        {
            Stop();
            if (OnFinish != null)
                OnFinish();
        }

        public void Stop()
        {
            IsMoving = false;
            if (_unitObject.NavMeshAgent.isOnNavMesh)
                _unitObject.NavMeshAgent.ResetPath();
            _unitObject.NavMeshAgent.avoidancePriority = StopPriority;
            _timeManager.StopUpdate(ref _update);
        }

        public void RegularUpdate(float dt)
        {
            if (!_unitObject.NavMeshAgent.enabled)
                return;
            if (_unitObject.NavMeshAgent.pathPending)
                return;

            UpdatePathComplete();
            UpdateReachedByNeighbour();
            UpdatePushedAway();
            UpdateGiveWay();
        }

        public bool IsUnitReachedTarget(Vector3 point, float epsilon = 1f)
        {
            return (_unitObject.Position - point).sqrMagnitude < epsilon;
        }

        public bool IsUnitOrNeighboursReachedTarget(Vector3 point, int hash = -1)
        {
            return IsUnitReachedTarget(point, hash == -1 ? 0.01f : 1f) || Neighbourhood.IsNeighboursReachedTarget(point, hash);
        }

        private void UpdatePathComplete()
        {
            if (_unitObject.NavMeshAgent.remainingDistance <= _unitObject.NavMeshAgent.stoppingDistance)
            {
                if (!_unitObject.NavMeshAgent.hasPath || _unitObject.NavMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    Finish();
                }
            }
        }

        private void UpdateReachedByNeighbour()
        {
            if (IsUnitOrNeighboursReachedTarget(_navMeshTarget))
                Finish();
        }

        private void UpdatePushedAway()
        {
            if (!_alreadyReachedTarget && IsUnitReachedTarget(_navMeshTarget))
            {
                _alreadyReachedTarget = true;
                _lastDistanceToTarget = _unitObject.NavMeshAgent.remainingDistance;
            }

            if (_alreadyReachedTarget && _unitObject.NavMeshAgent.remainingDistance > _lastDistanceToTarget)
                Finish();
        }

        private void UpdateGiveWay()
        {
            var opposingPriority = Neighbourhood.GetOppositeNeighbourPriority();
            if (opposingPriority != -1)
            {
                if (_unitObject.NavMeshAgent.avoidancePriority != GiveWayPriority && opposingPriority != GiveWayPriority)
                    _unitObject.NavMeshAgent.avoidancePriority = GiveWayPriority;
            }
            else
            {
                if (_unitObject.NavMeshAgent.avoidancePriority == GiveWayPriority)
                    _unitObject.NavMeshAgent.avoidancePriority = MovePriority;
            }
        }
    }
}
