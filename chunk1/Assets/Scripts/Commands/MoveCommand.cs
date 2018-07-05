using System.Collections;
using Assets.Scripts.Formations;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Commands
{
    public class MoveCommand : CommandBase
    {
        Vector3 _initialTarget;

        public override CommandType GetKey() { return CommandType.Move; }

        public MoveCommand() : base()
        {
        }

        public override void Init(Vector3 target)
        {
            _initialTarget = target;
            base.Init(target);
        }

        public override void Start(Unit unit)
        {
            base.Start(unit);

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
