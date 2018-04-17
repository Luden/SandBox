﻿using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using System;

public class SelectionManager : MonoBehaviour
{
    [SerializeField]
    UnitsManager _unitsManager;
    Camera _mainCamera;

	public Action<List<ISelectable>, List<ISelectable>, List<ISelectable>> OnSelectionChange;

	private List<ISelectable> _selectedUnits = new List<ISelectable>();
	private List<ISelectable> _recentlySelectedUnits = new List<ISelectable>();
	private List<ISelectable> _recentlyDeselectedUnits = new List<ISelectable>();

	void Start()
	{
        _mainCamera = Camera.main;
    }

	public void ProcessSelection(Vector3 screenPos1, Vector3 screenPos2)
	{
		_selectedUnits.Clear();
		_recentlySelectedUnits.Clear();
		_recentlyDeselectedUnits.Clear();

		foreach (var selectable in _unitsManager.Selectables)
        {
            var unitScreenPos = _mainCamera.WorldToScreenPoint(selectable.Position);
            var inRect = unitScreenPos.x > screenPos1.x && unitScreenPos.x < screenPos2.x
                && unitScreenPos.y > screenPos1.y && unitScreenPos.y < screenPos2.y;

			if (!selectable.Selected && inRect)
			{
				_recentlySelectedUnits.Add(selectable);
				selectable.Selected = true;
			}
			else if (selectable.Selected && !inRect)
			{
				_recentlyDeselectedUnits.Add(selectable);
				selectable.Selected = false;
			}

			if (selectable.Selected)
				_selectedUnits.Add(selectable);
		}

        ProcessAdditionalSingleSelection(screenPos1);
        ProcessAdditionalSingleSelection(screenPos2);

		if (_recentlySelectedUnits.Count > 0 || _recentlyDeselectedUnits.Count > 0 && OnSelectionChange != null)
			OnSelectionChange(_selectedUnits, _recentlySelectedUnits, _recentlyDeselectedUnits);
	}

	public ISelectable GetUnitUnderCursor(Vector3 screenPos)
	{
		RaycastHit hit;
		var ray = _mainCamera.ScreenPointToRay(screenPos);
		if (Physics.Raycast(ray, out hit, 1000, ~LayerMask.NameToLayer("Units")))
			return hit.transform.GetComponent<Selectable>();
		return null;
	}

	public Vector3 TraceTerrain(Vector3 screenPos)
	{
		RaycastHit hit;
		var ray = _mainCamera.ScreenPointToRay(screenPos);
		if (Physics.Raycast(ray, out hit, 1000, ~LayerMask.NameToLayer("Ground")))
			return hit.point;

		return Vector3.zero;
	}

    public void ProcessAdditionalSingleSelection(Vector3 screenPos)
    {
		var selectable = GetUnitUnderCursor(screenPos);
		if (selectable != null && !selectable.Selected)
		{
			selectable.Selected = true;
			_selectedUnits.Add(selectable);
			_recentlySelectedUnits.Add(selectable);
			_recentlyDeselectedUnits.Remove(selectable);
		}
    }

	public bool HasSelectedUnits()
	{
		return _selectedUnits.Count > 0;
	}
}