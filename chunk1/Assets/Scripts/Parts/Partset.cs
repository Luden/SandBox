using System;
using System.Collections.Generic;

namespace Assets.Scripts.Parts
{
    public class Partset
    {
        public Action<Part, int> OnPartAttached;
        public Action<Part, int> OnPartDetached;

        public Dictionary<int, Part> Parts = new Dictionary<int, Part>();

        private int _slotsCount = 9;

        public Partset()
        {
            for (int i = 0; i < _slotsCount; i++)
                Parts[i] = null;
        }

        public void AttachPart(Part part)
        {
            var slot = GetFreeSlot();
            if (slot == -1)
                return;

            AttachPart(slot, part);
        }

        public void AttachPart(int slot, Part part)
        {
            Part existingPart;
            Parts.TryGetValue(slot, out existingPart);
            if (existingPart != null)
                return;
            if (GetPartIndex(part) != -1)
                return;

            Parts[slot] = part;

            if (OnPartAttached != null)
                OnPartAttached(part, slot);
        }

        public void DetachPart(int slot)
        {
            Part part;
            Parts.TryGetValue(slot, out part);
            if (part == null)
                return;

            Parts[slot] = null;
            if (OnPartDetached != null)
                OnPartDetached(part, slot);
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

            foreach (var pair in Parts)
            {
                if (pair.Value == part)
                    return pair.Key;
            }
            return -1;
        }

        private int GetFreeSlot()
        {
            foreach (var pair in Parts)
            {
                if (pair.Value == null)
                    return pair.Key;
            }
            return -1;
        }
    }
}
