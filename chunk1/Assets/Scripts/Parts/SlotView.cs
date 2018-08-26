using UnityEngine;

namespace Assets.Scripts.Parts
{
    public class SlotView : MonoBehaviour
    {
        public PartView PartView;
        public Transform PartRoot;

        private void Start()
        {
            PartView = PartRoot.GetComponentInChildren<PartView>();
        }
    }
}