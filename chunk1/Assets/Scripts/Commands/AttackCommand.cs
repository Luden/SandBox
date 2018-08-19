using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    public class AttackCommand : CommandBase
    {
        public override CommandType GetKey() { return CommandType.Attack; }

        private Unit _target;

        public override void Init(Unit unit, Vector3 targetPos, Unit targetUnit)
        {
            _target = targetUnit;
            base.Init(unit, targetPos, targetUnit);
        }

        public override void Start()
        {
            base.Start();
            Unit.Following.SetTarget(_target);
            Unit.Targeting.SetTarget(_target);
        }

        protected override void Stop()
        {
            base.Stop();
            Unit.Targeting.Stop();
            Unit.Following.Stop();
        }

        public override void Update(float dt)
        {
            if (_target == null || _target.Hull.IsDead)
                Finish();
        }
    }
}
