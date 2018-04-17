using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Commands
{
	public class MoveCommand : CommandBase
	{
		Vector3 _initialTarget;
		Vector3 _navMeshTarget;

		public override CommandType GetCommandType() { return CommandType.Move; }

		public override void Init(Vector3 target)
		{
			_navMeshTarget = _initialTarget = target;
			base.Init(target);
		}

		public override void Start(Unit unit)
		{
			NavMeshHit hit;
			if (!NavMesh.SamplePosition(_initialTarget, out hit, 10f, 0))
			{
				Cancel();
				return;
			}

			_navMeshTarget = hit.position;
			unit.NavMeshAgent.SetDestination(_navMeshTarget);

			base.Start(unit);
		}

		public override void Update(Unit unit)
		{
			if (!unit.NavMeshAgent.pathPending)
			{
				if (unit.NavMeshAgent.remainingDistance <= unit.NavMeshAgent.stoppingDistance)
				{
					if (!unit.NavMeshAgent.hasPath || unit.NavMeshAgent.velocity.sqrMagnitude == 0f)
					{
						Finish();
					}
				}
			}
			base.Update(unit);
		}
	}
}
