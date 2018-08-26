using System;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Core;
using Assets.Scripts.Parts;
using Assets.Scripts.Units;
using UnityEngine;

public class UnitManager : IManager
{
    private static int _lastID;

    public ManagerType ManagerType { get { return ManagerType.Unit; } }
    public List<Unit> Units = new List<Unit>();
    public Action<Unit> OnUnitCreated;

    private UnitObjectManager _unitObjectManager;
    private PartsManager _partsManager;
    private Dictionary<int, Unit> _units = new Dictionary<int, Unit>();

    public void Init()
    {
        _unitObjectManager = ManagerProvider.Instance.UnitObjectManager;
        _partsManager = ManagerProvider.Instance.PartsManager;
    }

    public void AddUnit()
    {
        var unitObject = _unitObjectManager.CreateUnitObject();
        AddUnit(unitObject, new Dictionary<int, PartType>());
    }

    public void AddUnit(IUnitObject unitObject, Dictionary<int, PartType> parts)
    {
        var unit = new Unit(++_lastID, unitObject, Faction.N);

        foreach (var pair in parts)
            unit.Partset.AttachPart(pair.Key, _partsManager.CreatePart(pair.Value));

        Units[_lastID] = unit;
        Units.Add(unit);

        if (OnUnitCreated != null)
            OnUnitCreated(unit);
    }
}
