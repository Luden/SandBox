using System.Collections.Generic;
using Assets.Scripts.Parts;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class UnitView : MonoBehaviour
    {
        public int Id;
        public TargetView TargetView;
        public Dictionary<int, SlotView> Slots = new Dictionary<int, SlotView>();

        private Unit _unit;
        private PartViewsManager _partViewsManager;

        public void Start()
        {
            var slots = GetComponentsInChildren<SlotView>();
            for (int i = 0; i < slots.Length; i++)
                Slots[i] = slots[i];

            ManagerProvider.Instance.UnitViewManager.OnUnitViewAppeared(this);
        }

        public void Init(Unit unit)
        {
            _unit = unit;
            Id = _unit.Id;

            _partViewsManager = ManagerProvider.Instance.PartViewsManager;

            _unit.Partset.OnPartAttached += OnPartAttached;
            _unit.Partset.OnPartDetached += OnPartDetached;

            TargetView = GetComponentInChildren<TargetView>();
            TargetView.Init(_unit);
        }

        public Dictionary<int, PartType> GetInstalledParts()
        {
            var result = new Dictionary<int, PartType>();
            foreach (var pair in Slots)
            {
                if (pair.Value.PartView != null)
                    result[pair.Key] = pair.Value.PartView.PartType;
            }
            return result;
        }

        private void OnPartAttached(Part part, int slotNum)
        {
            SlotView slot;
            Slots.TryGetValue(slotNum, out slot);
            if (slot == null)
                return;

            if (slot.PartView != null && slot.PartView.PartType != part.Type)
            {
                _partViewsManager.ReleaseView(slot.PartView);
                slot.PartView = null;
            }

            if (slot.PartView == null)
                slot.PartView = _partViewsManager.CreateView(part.Type, slot.PartRoot);

            slot.PartView.Init(part);
        }

        private void OnPartDetached(Part part, int slotNum)
        {
            SlotView slot;
            Slots.TryGetValue(slotNum, out slot);
            if (slot == null || slot.PartView == null)
                return;

            _partViewsManager.ReleaseView(slot.PartView);
            slot.PartView = null;
        }
    }
}
