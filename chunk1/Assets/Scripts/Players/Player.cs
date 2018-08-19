using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Core;

namespace Assets.Scripts.Players
{
    public class Player
    {
        public Faction Faction;

        public Player(Faction faction)
        {
            Faction = faction;
        }

        public bool IsEnemy()
        {
            return Faction == Faction.B;
        }
    }
}
