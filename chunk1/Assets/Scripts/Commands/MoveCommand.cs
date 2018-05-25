using System.Collections;
using Assets.Scripts.Formations;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Commands
{
    public class MoveCommand : CommandBase
	{
		Vector3 _initialTarget;
		Vector3 _navMeshTarget;

        const int StopPriority = 1;
        const float StopCheckSpeedPart = 0.1f;
        const float StopTime = 1f;

        float _stopCheckSpeedSquared = 0f;
        float _stopingTime = 0f;

        public override CommandType GetKey() { return CommandType.Move; }

        TimeManager _timeManager;

        public MoveCommand(TimeManager timeManager) : base()
        {
            _timeManager = timeManager;
        }

        public override void Init(Vector3 target)
		{
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

            _timeManager.LateUpdateOnce(LateUpdate);
        }

        private void LateUpdate(float t)
        {
            //Unit.NavMeshAgent.enabled = true;
            Unit.NavMeshAgent.SetDestination(_navMeshTarget);
            Unit.NavMeshAgent.avoidancePriority = 50; // Random.Range(10, 90);
            _stopCheckSpeedSquared = Unit.NavMeshAgent.speed * Unit.NavMeshAgent.speed * StopCheckSpeedPart;
            _stopingTime = 0f;
        }

        protected override void Stop()
        {
            if (Unit.NavMeshAgent.isOnNavMesh)
                Unit.NavMeshAgent.ResetPath();
            Unit.NavMeshAgent.avoidancePriority = 50;
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

			if (Unit.NavMeshAgent.remainingDistance <= Unit.NavMeshAgent.stoppingDistance)
			{
				if (!Unit.NavMeshAgent.hasPath || Unit.NavMeshAgent.velocity.sqrMagnitude == 0f)
				{
					Finish();
				}
			}

            if (Unit.NavMeshAgent.velocity.sqrMagnitude < _stopCheckSpeedSquared)
            {
                _stopingTime += dt;
                if (_stopingTime > StopTime)
                    Finish();
            }
            else
            {
                _stopingTime = 0f;
            }
		}
	}
}
