using System;
using System.Collections.Generic;

namespace Assets.Scripts.Parts
{
    public class Partset
    {
        public Action<Slot> OnPartAttached;
        public Action<Part, int> OnPartDetached;

        public Dictionary<int, Slot> Slots = new Dictionary<int, Slot>();

        public Partset()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var slot = new Slot();
                    slot.Offset = new UnityEngine.Vector3((i - 1) * 0.33f, 0.569f - 0.049f, (j - 1) * 0.33f);
                    slot.Index = i * 3 + j;
                    Slots[i * 3 + j] = slot;
                }
            }
        }

        public void AttachPart(Part part)
        {
            var slot = GetFreeSlot();
            if (slot == -1)
                return;

            AttachPart(slot, part);
        }

        public void AttachPart(int slotNum, Part part)
        {
            Slot slot;
            Slots.TryGetValue(slotNum, out slot);
            if (slot == null || slot.Part != null)
                return;
            if (GetPartIndex(part) != -1)
                return;

            slot.Part = part;

            if (OnPartAttached != null)
                OnPartAttached(slot);
        }

        public void DetachPart(int slotNum)
        {
            Slot slot;
            Slots.TryGetValue(slotNum, out slot);
            if (slot == null || slot.Part == null)
                return;

            var part = slot.Part;
            Slots[slotNum].Part = null;
            if (OnPartDetached != null)
                OnPartDetached(part, slotNum);
        }

        public void DetachPart(Part part)
        {
            var index = GetPartIndex(part);
            DetachPart(index);
        }

        private int GetPartIndex(Part part)
        {
            if (part == null)
                return -1;

            foreach (var pair in Slots)
            {
                if (pair.Value.Part == part)
                    return pair.Key;
            }
            return -1;
        }

        private int GetFreeSlot()
        {
            foreach (var pair in Slots)
            {
                if (pair.Value.Part == null)
                    return pair.Key;
            }
            return -1;
        }
    }
}
