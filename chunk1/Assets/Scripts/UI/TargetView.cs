using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class TargetView : MonoBehaviour
    {
        [SerializeField]
        private Unit _unit;
        private Unit _lastTarget;
        private TargetView _lastTargetView;

        private MeshRenderer _visual;

        void Start()
        {
            _unit.Selectable.OnSelectionChange += UpdateTarget;
            _unit.Targeting.OnTargetChange += UpdateTarget;

            _visual = GetComponent<MeshRenderer>();
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

                if (_lastTarget != null)
                {
                    if (_lastTargetView != null)
                        _lastTargetView.SetTargeted(false);
                    _lastTargetView = _lastTarget.GetComponentInChildren<TargetView>();
                }
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
