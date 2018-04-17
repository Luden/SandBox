using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Commands
{
	public class CommandManager : MonoBehaviour
	{
		public CommandFactory CommandFactory { get; private set; }

		private UnitsManager _unitsManager;

		private void Start()
		{
			_unitsManager = ManagerProvider.Instance.UnitManager;
			CommandFactory = new CommandFactory();
		}

		public void Send(CommandType commandType, Vector3 target)
		{
			foreach (var processor in _unitsManager.CommandReceivers)
			{
				var command = CommandFactory.GetOrCreateCommand(commandType);
				command.Init(target);
				processor.Add(command);
			}
		}
	}
}
