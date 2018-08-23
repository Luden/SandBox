using UnityEngine;

namespace Assets.Scripts.Core
{
    public interface IManager
    {
        void Init();
        ManagerType ManagerType { get; }
    }
}
