using System;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Core;
using Assets.Scripts.Units;
using UnityEngine;

public class UnitManager : IManager
{
    private static int _lastID;

    public ManagerType ManagerType { get { return ManagerType.Unit; } }
    public List<Unit> Units = new List<Unit>();
    public Action<Unit> OnUnitCreated;

    private UnitObjectManager _unitObjectManager;
    private Dictionary<int, Unit> _units = new Dictionary<int, Unit>();

    public void Init()
    {
        _unitObjectManager = ManagerProvider.Instance.UnitObjectManager;
    }

    public void PostInit()
    {
        var unitObjects = _unitObjectManager.GetInitialUnits();
        foreach (var unit in unitObjects)
            AddUnit(unit);
    }

    private void AddUnit(IUnitObject unitObject)
    {
        var unit = new Unit(++_lastID, unitObject, Faction.N);
        Units[_lastID] = unit;
        Units.Add(unit);
        if (OnUnitCreated != null)
            OnUnitCreated(unit);
    }
}
