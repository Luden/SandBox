﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Units;

namespace Assets.Scripts.Commands
{
    public class CommandProcessor
    {
        private Queue<CommandBase> _commands = new Queue<CommandBase>();
        private CommandBase _currentCommand;

        private CommandFactory _commandFactory;
        private RegularUpdate _update;
        private TimeManager _timeManager;

        public CommandProcessor(CommandFactory commandFactory, TimeManager timeManager, float updatePeriod, Unit owner)
        {
            _commandFactory = commandFactory;
            _timeManager = timeManager;
        }

        public void Stop()
        {
            _timeManager.StopUpdate(ref _update);
        }

        public void Add(CommandBase command)
        {
            _commands.Enqueue(command);
            TryStartCommand();
            _timeManager.StartUpdate(ref _update, Update, 0.1f);
        }

        public bool IsMoving()
        {
            return _currentCommand != null && _currentCommand.GetKey() == CommandType.Move;
        }

        public void Clear()
        {
            if (_currentCommand != null)
            {
                _currentCommand.Cancel();
                _commandFactory.Release(_currentCommand);
                _currentCommand = null;
            }
            foreach (var command in _commands)
            {
                command.Cancel();
                _commandFactory.Release(command);
            }
            _commands.Clear();
        }

        private void TryStartCommand()
        {
            if (_currentCommand != null)
                return;

            if (_commands.Count == 0)
                return;

            _currentCommand = _commands.Dequeue();
            _currentCommand.Start();
        }

        public void Update(float dt)
        {
            TryStartCommand();

            if (_currentCommand == null)
            {
                _timeManager.StopUpdate(ref _update);
                return;
            }

			if (_currentCommand.State != CommandState.Canceled)
				_currentCommand.Update(dt);

			if (_currentCommand.State == CommandState.Canceled || _currentCommand.State == CommandState.Finished)
			{
				_commandFactory.Release(_currentCommand);
				_currentCommand = null;
			}
		}
	}
}
