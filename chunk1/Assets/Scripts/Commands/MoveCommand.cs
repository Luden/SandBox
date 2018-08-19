using System.Collections;
using Assets.Scripts.Formations;
using Assets.Scripts.Units;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Commands
{
    public class MoveCommand : CommandBase
    {
        Vector3 _initialTarget;

        public override CommandType GetKey() { return CommandType.Move; }

        public override void Init(Unit unit, Vector3 targetPos, Unit targetUnit)
        {
            _initialTarget = targetPos;
            base.Init(unit, targetPos, targetUnit);
        }

        public override void Start()
        {
            base.Start();

            Unit.Navigation.OnCancel += OnMoveCancel;
            Unit.Navigation.OnFinish += OnMoveFinish;
            Unit.Navigation.Go(_initialTarget);
        }

        private void OnMoveCancel()
        {
            Cancel();
        }

        private void OnMoveFinish()
        {
            Finish();
        }

        protected override void Stop()
        {
            Unit.Navigation.OnCancel -= OnMoveCancel;
            Unit.Navigation.OnFinish -= OnMoveFinish;

            Unit.Navigation.Stop();
            base.Stop();
        }
	}
}
