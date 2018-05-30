﻿using System.Collections;
using Assets.Scripts.Formations;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Commands
{
    public class MoveCommand : CommandBase
	{
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
        RegularUpdate _lateUpdate;
        public override CommandType GetKey() { return CommandType.Move; }

        TimeManager _timeManager;

        public MoveCommand(TimeManager timeManager) : base()
        {
            _timeManager = timeManager;
        }

        public override void Init(Vector3 target)
		{
            _alreadyReachedTarget = false;
            _navMeshTarget = _initialTarget = target;
			base.Init(target);
		}

		public override void Start(Unit unit)
		{
            base.Start(unit);

            NavMeshHit hit;
			if (!NavMesh.SamplePosition(_initialTarget, out hit, 100f, NavMesh.AllAreas))
			{
				Cancel();
				return;
			}

			_navMeshTarget = hit.position;
            //unit.NavMeshObstacle.enabled = false;

            _lateUpdate = _timeManager.LateUpdateOnce(LateUpdate);
        }

        private void LateUpdate(float t)
        {
            //Unit.NavMeshAgent.enabled = true;
            _lateUpdate = null;
            Unit.NavMeshAgent.SetDestination(_navMeshTarget);
            Unit.NavMeshAgent.avoidancePriority = MovePriority;
            _stopCheckSpeedSquared = Unit.NavMeshAgent.speed * Unit.NavMeshAgent.speed * StopCheckSpeedPart;
        }

        protected override void Stop()
        {
            if (Unit.NavMeshAgent.isOnNavMesh)
                Unit.NavMeshAgent.ResetPath();
            Unit.NavMeshAgent.avoidancePriority = StopPriority;

            if (_lateUpdate != null)
            {
                _timeManager.StopUpdate(_lateUpdate);
                _lateUpdate = null;
            }
            //Unit.NavMeshAgent.enabled = false;
            //Unit.NavMeshObstacle.enabled = true;
            base.Stop();
        }

        public override void Update(float dt)
		{
            base.Update(dt);

            if (!Unit.NavMeshAgent.enabled)
                return;

            if (Unit.NavMeshAgent.pathPending)
                return;

            if (_lateUpdate != null)
                return;

            UpdatePathComplete();

            UpdateReachedByNeighbour();

            UpdatePushedAway();

            UpdateGiveWay();
        }

        private void UpdatePathComplete()
        {
            if (Unit.NavMeshAgent.remainingDistance <= Unit.NavMeshAgent.stoppingDistance)
            {
                if (!Unit.NavMeshAgent.hasPath || Unit.NavMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    Finish();
                }
            }
        }

        private void UpdateReachedByNeighbour()
        {
            if (Unit.IsNeighboursReachedTarget(_navMeshTarget))
                Finish();
        }

        private void UpdatePushedAway()
        {
            if (!_alreadyReachedTarget && Unit.IsUnitReachedTarget(_navMeshTarget))
            {
                _alreadyReachedTarget = true;
                _lastDistanceToTarget = Unit.NavMeshAgent.remainingDistance;
            }

            if (_alreadyReachedTarget && Unit.NavMeshAgent.remainingDistance > _lastDistanceToTarget)
                Finish();
        }

        private void UpdateGiveWay()
        {
            var opposer = Unit.GetOppositeNeighbour();
            if (opposer != null)
            {
                if (Unit.NavMeshAgent.avoidancePriority != GiveWayPriority
                    && opposer.NavMeshAgent.avoidancePriority != GiveWayPriority)
                    Unit.NavMeshAgent.avoidancePriority = GiveWayPriority;
            }
            else
            {
                if (Unit.NavMeshAgent.avoidancePriority == GiveWayPriority)
                    Unit.NavMeshAgent.avoidancePriority = MovePriority;
            }
        }
	}
}
