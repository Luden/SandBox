using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    public class AttackCommand : CommandBase
    {
        public override CommandType GetKey() { return CommandType.Attack; }

        private Unit _target;

        public override void Init(Unit owner, Unit target)
        {
            _target = target;
            base.Init(owner, target);
        }

        public override void Start()
        {
            base.Start();
        }
    }
}
