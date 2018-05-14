using System.Linq;
using System.Collections.Generic;
using Assets.Scripts.Core;
using Assets.Scripts.Formations;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    public class CommandManager : ManagerBase
	{
		public CommandFactory CommandFactory { get; private set; }
        public FormationFactory FormationFactory { get; private set; }

        private UnitManager _unitsManager;

        private List<Unit> _units = new List<Unit>();

        public override ManagerType ManagerType { get { return ManagerType.Command; } }

        public override void Init()
		{
			_unitsManager = ManagerProvider.Instance.UnitManager;
			CommandFactory = new CommandFactory(ManagerProvider.Instance.TimeManager);
            FormationFactory = new FormationFactory();
        }

		public void Send(CommandType commandType, Vector3 target, bool queue)
		{
            _units.Clear();
            _units.AddRange(_unitsManager.Units.Where(unit => unit.Selectable.Selected));

            var formation = FormationFactory.GetOrCreate(FormationFactory.GetFormationType(commandType));
            formation.Init(_units.Select(x => x.transform.position), target);

            foreach (var unit in _units)
			{
                if (!queue)
                    unit.CommandProcessor.Clear();

                var command = CommandFactory.GetOrCreate(commandType);
                command.Init(target, formation);

                unit.CommandProcessor.Add(command);
			}
		}
	}
}
