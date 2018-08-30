using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Core;

namespace Assets.Scripts.Commands
{
    public class CommandFactory : Pool<CommandType, CommandBase>
    {
        public CommandFactory(TimeManager timeManager) : base()
        {
        }

        protected override CommandBase Create(CommandType commandType)
		{
			switch (commandType)
			{
				case CommandType.Move: return new MoveCommand();
                case CommandType.Attack: return new AttackCommand();
                default: return null;
			}
		}
	}
}
