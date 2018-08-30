using System;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Core;
using Assets.Scripts.Units;
using UnityEngine;

public class SelectionManager : IManager
{
    public Unit UnitUnderCursor { get; private set; }
    public Action<List<Unit>, List<Unit>, List<Unit>> OnSelectionChange;
    public IEnumerable<Unit> SelectedUnits { get { return _selectedUnits; } }
    public ManagerType ManagerType { get { return ManagerType.Selection; } }

    private UnitManager _unitsManager;
    private Camera _mainCamera;
    private List<Unit> _selectedUnits = new List<Unit>();
	private List<Unit> _recentlySelectedUnits = new List<Unit>();
	private List<Unit> _recentlyDeselectedUnits = new List<Unit>();

    public void Init()
	{
        _mainCamera = Camera.main;
        _unitsManager = ManagerProvider.Instance.UnitManager;
    }

    public void ProcessPreselection(Vector3 screenPos1, Vector3 screenPos2)
    {
        foreach (var unit in _unitsManager.Units)
        {
            var inRect = CheckInRect(screenPos1, screenPos2, unit);
            if (!unit.Selectable.Preselected && inRect)
                unit.Selectable.Preselected = true;
            else if (unit.Selectable.Preselected && !inRect)
                unit.Selectable.Preselected = false;
        }
    }

    private bool CheckInRect(Vector3 screenPos1, Vector3 screenPos2, Unit unit)
    {
        var unitScreenPos = _mainCamera.WorldToScreenPoint(unit.Navigation.Position);
        return unitScreenPos.x + unit.Selectable.Radius > screenPos1.x && unitScreenPos.x - unit.Selectable.Radius < screenPos2.x
            && unitScreenPos.y + unit.Selectable.Radius > screenPos1.y && unitScreenPos.y - unit.Selectable.Radius < screenPos2.y;
    }

    public void ProcessPreselection(Vector3 screenPos)
    {
        UnitUnderCursor = GetUnitUnderCursor(screenPos);
        foreach (var unit in _unitsManager.Units)
            unit.Selectable.Preselected = unit == UnitUnderCursor;
    }

    public void ProcessSelection(Vector3 screenPos1, Vector3 screenPos2)
	{
		_selectedUnits.Clear();
		_recentlySelectedUnits.Clear();
		_recentlyDeselectedUnits.Clear();

		foreach (var unit in _unitsManager.Units)
        {
            var inRect = CheckInRect(screenPos1, screenPos2, unit);
            if (!unit.Selectable.Selected && inRect)
			{
				_recentlySelectedUnits.Add(unit);
                unit.Selectable.Selected = true;
			}
			else if (unit.Selectable.Selected && !inRect)
			{
				_recentlyDeselectedUnits.Add(unit);
                unit.Selectable.Selected = false;
			}

			if (unit.Selectable.Selected)
				_selectedUnits.Add(unit);
		}

        ProcessAdditionalSingleSelection(screenPos1);
        ProcessAdditionalSingleSelection(screenPos2);

		if (_recentlySelectedUnits.Count > 0 || _recentlyDeselectedUnits.Count > 0 && OnSelectionChange != null)
			OnSelectionChange(_selectedUnits, _recentlySelectedUnits, _recentlyDeselectedUnits);
	}

	public Unit GetUnitUnderCursor(Vector3 screenPos)
	{
		RaycastHit hit;
		var ray = _mainCamera.ScreenPointToRay(screenPos);
		if (Physics.Raycast(ray, out hit, 1000f, (1 << LayerMask.NameToLayer("Units"))))
			return hit.transform.GetComponent<UnitObject>().Owner;
		return null;
	}

	public Vector3 TraceTerrain(Vector3 screenPos)
	{
		RaycastHit hit;
		var ray = _mainCamera.ScreenPointToRay(screenPos);
		if (Physics.Raycast(ray, out hit, 1000f, (1 << LayerMask.NameToLayer("Terrain"))))
			return hit.point;

		return Vector3.zero;
	}

    public void ProcessAdditionalSingleSelection(Vector3 screenPos)
    {
		var unit = GetUnitUnderCursor(screenPos);
		if (unit != null && !unit.Selectable.Selected)
		{
			unit.Selectable.Selected = true;
			_selectedUnits.Add(unit);
			_recentlySelectedUnits.Add(unit);
			_recentlyDeselectedUnits.Remove(unit);
		}
    }

	public bool HasSelectedUnits()
	{
		return _selectedUnits.Count > 0;
	}

    
}
