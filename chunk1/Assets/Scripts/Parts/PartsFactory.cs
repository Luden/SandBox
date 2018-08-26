using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Weapons;

namespace Assets.Scripts.Parts
{
    public class PartsFactory
    {
        public Part Create(PartType partType)
        {
            switch (partType)
            {
                case PartType.Gun: return new Gun();
                default: return null;
            }
        }
    }
}
