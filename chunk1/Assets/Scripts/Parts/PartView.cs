using UnityEngine;

namespace Assets.Scripts.Parts
{
    public class PartView : MonoBehaviour
    {
        public PartType PartType;

        protected Part _part;

        public virtual void Init(Part part)
        {
            _part = part;
        }
    }
}
