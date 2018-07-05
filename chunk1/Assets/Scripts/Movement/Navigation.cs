using System;
using Assets.Scripts;
using Assets.Scripts.Commands;
using Assets.Scripts.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Movement
{
    public class Navigation : MonoBehaviour
    {
        public NavMeshAgent NavMeshAgent;
        public Neighbourhood Neighbourhood;

        public Action OnCancel;
        public Action OnFinish;

        RegularUpdate _update;
        TimeManager _timeManager;

        Vector3 _initialTarget;
        Vector3 _navMeshTarget;

        const int StopPriority = 50;
        const int MovePriority = 48;
        const int GiveWayPriority = 49;
        const float StopCheckSpeedPart = 0.1f;
        const float StopTime = 1f;

        float _stopCheckSpeedSquared = 0f;
        bool _alreadyReachedTarget = false;
        float _lastDistanceToTarget = 0f;

        public void Start()
        {
            var provider = ManagerProvider.Instance;
            _timeManager = provider.TimeManager;
            NavMeshAgent = GetComponent<NavMeshAgent>();
            Neighbourhood = GetComponent<Neighbourhood>();
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

            NavMeshAgent.SetDestination(_navMeshTarget);
            NavMeshAgent.avoidancePriority = MovePriority;
            _stopCheckSpeedSquared = NavMeshAgent.speed * NavMeshAgent.speed * StopCheckSpeedPart;
            _update = _timeManager.StartUpdate(RegularUpdate, 0.1f);
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
            if (NavMeshAgent.isOnNavMesh)
                NavMeshAgent.ResetPath();
            NavMeshAgent.avoidancePriority = StopPriority;

            if (_update != null)
            {
                _timeManager.StopUpdate(_update);
                _update = null;
            }
        }

        public void RegularUpdate(float dt)
        {
            if (!NavMeshAgent.enabled)
                return;
            if (NavMeshAgent.pathPending)
                return;

            UpdatePathComplete();
            UpdateReachedByNeighbour();
            UpdatePushedAway();
            UpdateGiveWay();
        }

        public bool IsUnitReachedTarget(Vector3 point)
        {
            return (transform.position - point).sqrMagnitude < 1f;
        }

        public bool IsUnitOrNeighboursReachedTarget(Vector3 point, int hash = -1)
        {
            return IsUnitReachedTarget(point) || Neighbourhood.IsNeighboursReachedTarget(point, hash);
        }

        private void UpdatePathComplete()
        {
            if (NavMeshAgent.remainingDistance <= NavMeshAgent.stoppingDistance)
            {
                if (!NavMeshAgent.hasPath || NavMeshAgent.velocity.sqrMagnitude == 0f)
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
                _lastDistanceToTarget = NavMeshAgent.remainingDistance;
            }

            if (_alreadyReachedTarget && NavMeshAgent.remainingDistance > _lastDistanceToTarget)
                Finish();
        }

        private void UpdateGiveWay()
        {
            var opposer = Neighbourhood.GetOppositeNeighbour();
            if (opposer != null)
            {
                if (NavMeshAgent.avoidancePriority != GiveWayPriority
                    && opposer.NavMeshAgent.avoidancePriority != GiveWayPriority)
                    NavMeshAgent.avoidancePriority = GiveWayPriority;
            }
            else
            {
                if (NavMeshAgent.avoidancePriority == GiveWayPriority)
                    NavMeshAgent.avoidancePriority = MovePriority;
            }
        }
    }
}
