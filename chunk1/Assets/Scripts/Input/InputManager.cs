using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.Input
{
    public class InputManager : MonoBehaviour, IManager
	{
		public SelectionRect SelectionRect;
		public KeyInput KeyInput;

        public ManagerType ManagerType { get { return ManagerType.Input; } }

        public void Init()
        {

        }
    }
}
