using System.Linq;
using System.Collections.Generic;
using Assets.Scripts.Core;
using Assets.Scripts.Formations;
using UnityEngine;
using Assets.Scripts.Units;

namespace Assets.Scripts.Commands
{
    public class CommandManager : IManager
	{
		public CommandFactory CommandFactory { get; private set; }
        public FormationFactory FormationFactory { get; private set; }
        public ManagerType ManagerType { get { return ManagerType.Command; } }

        private SelectionManager _selectionManager;
        private List<Unit> _units = new List<Unit>();

        public void Init()
		{
            _selectionManager = ManagerProvider.Instance.SelectionManager;
            CommandFactory = new CommandFactory(ManagerProvider.Instance.TimeManager);
            FormationFactory = new FormationFactory();
        }

        public void Send(CommandType commandType, Vector3 targetPos, Unit targetUnit, bool queue)
		{
            _units.Clear();
            _units.AddRange(_selectionManager.SelectedUnits);

            var formation = FormationFactory.GetOrCreate(
                FormationSelector.GetFormationType(_units, targetPos, commandType));
            formation.Init(_units.Select(x => x.Navigation.Position), targetPos);

            int index = 0;
            foreach (var unit in _units)
			{
                if (!queue)
                    unit.CommandProcessor.Clear();

                var command = CommandFactory.GetOrCreate(commandType);
                command.Init(unit, formation.GetTargePos(index++), targetUnit);

                unit.CommandProcessor.Add(command);
			}

            //_lastFormation = formation;
            FormationFactory.Release(formation);
        }

        //private Formation _lastFormation;
        //private List<Color> _colors = new List<Color>();
        //private void OnDrawGizmosSelected()
        //{
        //    if (_lastFormation == null)
        //        return;

        //    int index = 0;
        //    foreach (var slot in _lastFormation.GetSlots())
        //    {
        //        if (index >= _colors.Count)
        //            _colors.Add(Random.ColorHSV());

        //        Gizmos.color = _colors[index++];
        //        Gizmos.DrawSphere(slot.TargetPos + new Vector3(0, 0.5f, 0), 0.5f);
        //        Gizmos.DrawSphere(slot.Pos + new Vector3(0, 0.5f, 0), 0.5f);
        //        Gizmos.DrawLine(slot.Pos + new Vector3(0, 0.5f, 0), slot.TargetPos + new Vector3(0, 0.5f, 0));
        //    }
        //}
    }
}
