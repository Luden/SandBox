using System.Collections.Generic;
using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class UnitViewManager : MonoBehaviour, IManager
    {
        public ManagerType ManagerType { get { return ManagerType.UnitView; } }

        public Dictionary<int, UnitView> Units = new Dictionary<int, UnitView>();

        public void Init()
        {
            var unitManager = ManagerProvider.Instance.UnitManager;
            unitManager.OnUnitCreated += OnUnitCreated;
        }

        private void OnUnitCreated(Unit unit)
        {
            var unitObject = unit.UnitObject;
            var view = (unitObject as UnitObject).GetComponent<UnitView>();
            view.Init(unit);
            Units[unit.Id] = view;
        }
    }
}
