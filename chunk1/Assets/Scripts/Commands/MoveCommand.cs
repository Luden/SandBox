using Assets.Scripts.Formations;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Commands
{
    public class MoveCommand : CommandBase
	{
		Vector3 _initialTarget;
		Vector3 _navMeshTarget;

        const int StopPriority = 51;
        const int MovePriority = 50;
        const float StopCheckSpeedPart = 0.1f;
        const float StopTime = 1f;

        float _stopCheckSpeedSquared = 0f;
        float _stopingTime = 0f;

        public override CommandType GetKey() { return CommandType.Move; }

		public override void Init(Vector3 target, Formation formation)
		{
			_navMeshTarget = _initialTarget = target;
			base.Init(target, formation);
		}

		public override void Start(Unit unit)
		{
			NavMeshHit hit;
			if (!NavMesh.SamplePosition(_initialTarget, out hit, 100f, NavMesh.AllAreas))
			{
				Cancel();
				return;
			}

			_navMeshTarget = hit.position;
			unit.NavMeshAgent.SetDestination(_navMeshTarget);
            unit.NavMeshAgent.avoidancePriority = MovePriority;
            _stopCheckSpeedSquared = unit.NavMeshAgent.speed * unit.NavMeshAgent.speed * StopCheckSpeedPart;
            _stopingTime = 0f;

            base.Start(unit);
		}

        protected override void Stop()
        {
            Unit.NavMeshAgent.ResetPath();
            Unit.NavMeshAgent.avoidancePriority = StopPriority;
            base.Stop();
        }

        public override void Update(float dt)
		{
            base.Update(dt);

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
