using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class TargetView : MonoBehaviour
    {
        private Unit _unit;
        private Unit _lastTarget;
        private TargetView _lastTargetView;
        private MeshRenderer _visual;
        private UnitViewManager _unitViewManager;

        public void Init(Unit unit)
        {
            _unit = unit;
            _unitViewManager = ManagerProvider.Instance.UnitViewManager;

            _unit.Selectable.OnSelectionChange += UpdateTarget;
            _unit.Targeting.OnTargetChange += UpdateTarget;

            _visual = GetComponent<MeshRenderer>();
            SetTargeted(false);
        }

        private void UpdateTarget(Unit Target)
        {
            UpdateTarget(true);
        }

        private void UpdateTarget(bool selected)
        {
            if (_unit.Targeting.CurrentTarget != _lastTarget)
            {
                _lastTarget = _unit.Targeting.CurrentTarget;

                if (_lastTargetView != null)
                    _lastTargetView.SetTargeted(false);

                if (_lastTarget != null)
                    _lastTargetView = _unitViewManager.Units[_lastTarget.Id].TargetView;
                else
                    _lastTargetView = null;
            }

            if (_lastTargetView != null)
                _lastTargetView.SetTargeted(selected);
        }

        public void SetTargeted(bool targeted)
        {
            if (_visual.enabled != targeted)
                _visual.enabled = targeted;
        }
    }
}
