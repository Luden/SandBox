using System.Collections.Generic;
using Assets.Scripts.Core;
using Assets.Scripts.Units;
using UnityEngine;

public class UnitManager : ManagerBase
{
    public List<Unit> Units = new List<Unit>();

    public override ManagerType ManagerType { get { return ManagerType.Unit; } }

    public override void Init()
    {
        Units.AddRange(GameObject.FindObjectsOfType<Unit>());
    }
}
