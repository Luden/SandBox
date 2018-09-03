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
    public Action<Unit> OnUnitDestroyed;

    private UnitObjectManager _unitObjectManager;
    private PartsManager _partsManager;
    //private Dictionary<int, Unit> _units = new Dictionary<int, Unit>();

    public void Init()
    {
        _unitObjectManager = ManagerProvider.Instance.UnitObjectManager;
        _partsManager = ManagerProvider.Instance.PartsManager;
    }

    public void AddUnit()
    {
        var unitObject = _unitObjectManager.CreateUnitObject();
        AddUnit(unitObject, Faction.N);
    }

    public void AddUnit(IUnitObject unitObject, Faction faction, Dictionary<int, PartType> parts = null)
    {
        var unit = new Unit(++_lastID, unitObject, faction);
        unitObject.Init(unit);

        if (parts != null)
            foreach (var pair in parts)
                unit.Partset.AttachPart(pair.Key, _partsManager.CreatePart(pair.Value));

        Units.Add(unit);
        unit.OnDeath += OnUnitDeath;

        if (OnUnitCreated != null)
            OnUnitCreated(unit);
    }

    private void OnUnitDeath(Unit unit)
    {
        Units.Remove(unit);
        if (OnUnitDestroyed != null)
            OnUnitDestroyed(unit);
    }

}
