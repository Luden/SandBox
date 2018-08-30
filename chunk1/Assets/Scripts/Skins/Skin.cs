using System.Collections.Generic;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.Skins
{
    public class Skin : MonoBehaviour
    {
        public List<Material> Materials = new List<Material>();
        public MeshRenderer Renderer;

        public void Awake()
        {
            var unitView = GetComponent<UnitView>();
            Renderer.material = Materials[(int)unitView.Faction];
        }
    }
}
