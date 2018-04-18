using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Commands
{
    public class CommandFactory
	{
		private Dictionary<CommandType, List<CommandBase>> _commandPool = new Dictionary<CommandType, List<CommandBase>>();

		public CommandBase GetOrCreateCommand(CommandType commandType)
		{
			var pool = GetOrCreatePool(commandType);
			if (pool.Count != 0)
			{
				var command = pool.First();
				pool.RemoveAt(0);
				return command;
			}

			return CreateCommand(commandType);
		}

		public CommandBase CreateCommand(CommandType commandType)
		{
			switch (commandType)
			{
				case CommandType.Move: return new MoveCommand();
				default: return null;
			}
		}

		private List<CommandBase> GetOrCreatePool(CommandType commandType)
		{
			List<CommandBase> pool;
			if (!_commandPool.TryGetValue(commandType, out pool))
			{
				pool = new List<CommandBase>();
				_commandPool[commandType] = pool;
			}
			return pool;
		}

		public void Release(CommandBase command)
		{
			var pool = GetOrCreatePool(command.GetCommandType());
			pool.Add(command);
			command.SetPooled();
		}
	}
}
