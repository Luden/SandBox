using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Commands
{
	public class CommandProcessor
	{
		private Queue<CommandBase> _commands = new Queue<CommandBase>();
		private CommandBase _currentCommand;

		private Unit _owner; // dont know how to remove cyclic dependance
		private CommandFactory _commandFactory;
		private RegularUpdate _update;
		private TimeManager _timeManager;

		public CommandProcessor(CommandFactory commandFactory, TimeManager timeManager, float updatePeriod, Unit owner)
		{
			_owner = owner;
			_commandFactory = commandFactory;
			_timeManager = timeManager;
			_update = _timeManager.StartUpdate(Update, updatePeriod);
		}

		public void Stop()
		{
			_timeManager.StopUpdate(_update);
		}

		public void Add(CommandBase command)
		{
			_commands.Enqueue(command);
			TryStartCommand();
		}

		private void TryStartCommand()
		{
			if (_currentCommand != null)
				return;

			if (_commands.Count == 0)
				return;

			_currentCommand = _commands.Dequeue();
			_currentCommand.Start(_owner);
		}

		public void Update()
		{
			TryStartCommand();

            if (_currentCommand == null)
                return;

			if (_currentCommand.State != CommandState.Canceled)
				_currentCommand.Update(_owner);

			if (_currentCommand.State == CommandState.Canceled || _currentCommand.State == CommandState.Finished)
			{
				_commandFactory.Release(_currentCommand);
				_currentCommand = null;
			}
		}
	}
}
