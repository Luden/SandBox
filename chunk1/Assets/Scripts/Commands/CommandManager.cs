using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    public class CommandManager : ManagerBase
	{
		public CommandFactory CommandFactory { get; private set; }

		private UnitManager _unitsManager;

        public override ManagerType ManagerType { get { return ManagerType.Command; } }

        public override void Init()
		{
			_unitsManager = ManagerProvider.Instance.UnitManager;
			CommandFactory = new CommandFactory();
		}

		public void Send(CommandType commandType, Vector3 target, bool queue)
		{
			foreach (var processor in _unitsManager.CommandReceivers)
			{
                if (!queue)
                    processor.Clear();

                var command = CommandFactory.GetOrCreateCommand(commandType);
				command.Init(target);

                processor.Add(command);
			}
		}
	}
}
