using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Core;

namespace Assets.Scripts.Commands
{
    public class CommandFactory : Pool<CommandType, CommandBase>
    {
		private Dictionary<CommandType, List<CommandBase>> _commandPool = new Dictionary<CommandType, List<CommandBase>>();

        private TimeManager _timeManager;

        public CommandFactory(TimeManager timeManager) : base()
        {
            _timeManager = timeManager;
        }

        protected override CommandBase Create(CommandType commandType)
		{
			switch (commandType)
			{
				case CommandType.Move: return new MoveCommand();
				default: return null;
			}
		}
	}
}
