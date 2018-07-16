using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Core;

namespace Assets.Scripts.Movement
{
    public class Following
    {
        private Unit _target;
        private Faction _faction;
        private TimeManager _timeManager;


        public Following(TimeManager timeManager)
        {
            _timeManager = timeManager;
        }

        public void SetFaction(Faction faction)
        {
            _faction = faction;
        }

        public void SetTarget(Unit target)
        {

        }
    }
}
