using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Core;

namespace Assets.Scripts.Parts
{
    public class PartViewsManager : MonoBehaviour, IManager
    {
        public ManagerType ManagerType { get { return ManagerType.PartViews; } }
        public List<PartView> _partViewPrefabs = new List<PartView>();

        public void Init()
        {
        }

        public PartView CreateView(PartType partType, Transform root)
        {
            var prefab = _partViewPrefabs.FirstOrDefault(x => x.PartType == partType);
            if (prefab == null)
                return null;

            return GameObject.Instantiate<PartView>(prefab, root);
        }

        public void ReleaseView(PartView view)
        {
            Destroy(view);
        }
    }
}
