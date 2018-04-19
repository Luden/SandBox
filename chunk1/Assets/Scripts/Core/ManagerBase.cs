using UnityEngine;

namespace Assets.Scripts.Core
{
    public class ManagerBase : MonoBehaviour
    {
        public virtual void Init() { }

        public virtual ManagerType ManagerType { get { return ManagerType.None; } }
    }
}
