using System.Collections.Generic;
using Assets.Scripts.Parts;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class UnitView : MonoBehaviour
    {
        public TargetView TargetView;
        public List<PartView> Parts = new List<PartView>();

        private Unit _unit;

        public void Init(Unit unit)
        {
            _unit = unit;

            TargetView = GetComponentInChildren<TargetView>();
            TargetView.Init(_unit);
        }
    }
}
