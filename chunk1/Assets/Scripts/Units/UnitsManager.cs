using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using Assets.Scripts.Commands;

public class UnitsManager : MonoBehaviour
{
    List<Unit> _units = new List<Unit>();

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

	void Start()
	{
        _units.AddRange(GameObject.FindObjectsOfType<Unit>());
	}
}
