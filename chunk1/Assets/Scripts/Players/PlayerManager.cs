using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Core;

namespace Assets.Scripts.Players
{
    public class PlayerManager : ManagerBase
    {
        public List<Player> Players;

        public override ManagerType ManagerType { get { return ManagerType.Player; } }

        public Player Neutral;
        public Player Enemy;
        public Player MyPlayer;

        public override void Init()
        {
            Neutral = new Player(Faction.N);
            Enemy = new Player(Faction.B);
            MyPlayer = new Player(Faction.A);
        }

        public Player GetPlayer(Faction faction)
        {
            switch (faction)
            {
                case Faction.A: return MyPlayer;
                case Faction.B: return Enemy;
                default: return Neutral;
            }
        }
    }
}
