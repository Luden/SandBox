using UnityEngine;

namespace Assets.Scripts.Core
{
    public class LayerMasks
    {
        public static readonly int Units;
        public static readonly int Terrain;

        static LayerMasks()
        {
            Units = (1 << LayerMask.NameToLayer("Units"));
            Terrain = (1 << LayerMask.NameToLayer("Terrain"));
        }
    }
}
