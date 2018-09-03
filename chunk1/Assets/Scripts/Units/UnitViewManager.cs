using System.Collections.Generic;
using Assets.Scripts.Core;
using Assets.Scripts.UI;
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

        [SerializeField] private SelectionView _selectionPrefab;
        [SerializeField] private PreselectionView _preselectionPrefab;
        [SerializeField] private TargetView _targetViewPrefab;

        public void Init()
        {
            _unitManager = ManagerProvider.Instance.UnitManager;
            _timeManager = ManagerProvider.Instance.TimeManager;
            _unitManager.OnUnitCreated += OnUnitCreated;
            _unitManager.OnUnitDestroyed += OnUnitDestroyed;
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
                _unitManager.AddUnit(view.GetComponentInChildren<UnitObject>(), view.Faction, view.GetInstalledParts());
            }
            _viewsToProcess.Clear();
            _timeManager.StopUpdate(ref _updateAddedViews);
        }

        private void OnUnitDestroyed(Unit unit)
        {
            UnitView view;
            Units.TryGetValue(unit.Id, out view);
            if (view == null)
                return;

            Units.Remove(unit.Id);
            Destroy(view.gameObject);
        }

        private void OnUnitCreated(Unit unit)
        {
            var unitObject = unit.UnitObject;
            var view = (unitObject as UnitObject).GetComponent<UnitView>();

            var selection = Instantiate<SelectionView>(_selectionPrefab, view.transform);
            selection.Init(unit);

            var preselection = Instantiate<PreselectionView>(_preselectionPrefab, view.transform);
            preselection.Init(unit);

            var target = Instantiate<TargetView>(_targetViewPrefab, view.transform);
            target.Init(unit);

            view.Init(unit);

            Units[unit.Id] = view;
        }
    }
}
