using System.Collections.Generic;
using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class UnitViewManager : MonoBehaviour, IManager
    {
        public ManagerType ManagerType { get { return ManagerType.UnitView; } }
        public Dictionary<int, UnitView> Units = new Dictionary<int, UnitView>();

        private UnitManager _unitManager;
        private TimeManager _timeManager;
        private RegularUpdate _updateAddedViews;
        private List<UnitView> _viewsToProcess = new List<UnitView>();

        public void Init()
        {
            _unitManager = ManagerProvider.Instance.UnitManager;
            _timeManager = ManagerProvider.Instance.TimeManager;
            _unitManager.OnUnitCreated += OnUnitCreated;
            ProcessAddedViews(0f);
        }

        public void OnUnitViewAppeared(UnitView unitView)
        {
            _viewsToProcess.Add(unitView);
            if (_timeManager != null && _updateAddedViews == null)
                _timeManager.StartUpdate(ref _updateAddedViews, ProcessAddedViews, 0.1f);
        }

        private void ProcessAddedViews(float dt)
        {
            foreach (var view in _viewsToProcess)
            {
                if (view.Id != 0 && Units.ContainsKey(view.Id))
                    continue;
                _unitManager.AddUnit(view.GetComponentInChildren<UnitObject>(), view.GetInstalledParts());
            }
            _viewsToProcess.Clear();
            _timeManager.StopUpdate(ref _updateAddedViews);
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
