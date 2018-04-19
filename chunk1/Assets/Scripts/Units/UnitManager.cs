using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Commands;
using Assets.Scripts.Core;
using UnityEngine;

public class UnitManager : ManagerBase
{
    List<Unit> _units = new List<Unit>();

    public override ManagerType ManagerType { get { return ManagerType.Unit; } }

    public override void Init()
    {
        _units.AddRange(GameObject.FindObjectsOfType<Unit>());
    }

    public IEnumerable<ISelectable> Selectables
    {
        get
        {
            foreach (var unit in _units)
                yield return unit.Selectable;
        }
    }

	public IEnumerable<CommandProcessor> CommandReceivers
	{
		get
		{
			foreach (var unit in _units)
			{
				if (unit.Selectable.Selected)
					yield return unit.CommandProcessor;
			}
		}
	}
}
