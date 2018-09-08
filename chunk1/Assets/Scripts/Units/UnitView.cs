using System;
using System.Collections.Generic;
using Assets.Scripts.Core;
using Assets.Scripts.Parts;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class UnitView : MonoBehaviour
    {
        [NonSerialized] public int Id;
        [NonSerialized] public TargetView TargetView;
        public Dictionary<int, SlotView> Slots = new Dictionary<int, SlotView>();
        public Faction Faction;

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
            foreach (var pair in _unit.Partset.Slots)
                if (pair.Value.Part != null)
                    OnPartAttached(pair.Value);

            TargetView = GetComponentInChildren<TargetView>();
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

        private void OnPartAttached(Slot slot)
        {
            SlotView slotView;
            Slots.TryGetValue(slot.Index, out slotView);
            if (slotView == null)
                return;

            if (slotView.PartView != null && slotView.PartView.PartType != slot.Part.Type)
            {
                _partViewsManager.ReleaseView(slotView.PartView);
                slotView.PartView = null;
            }

            if (slotView.PartView == null)
                slotView.PartView = _partViewsManager.CreateView(slot.Part.Type, slotView.PartRoot);

            slotView.PartView.Init(slot.Part);
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
