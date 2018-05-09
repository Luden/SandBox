using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Commands;
using Assets.Scripts.Core;
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
