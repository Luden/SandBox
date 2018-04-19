using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.Input
{
    public class InputManager : ManagerBase
	{
		public SelectionRect SelectionRect;
		public KeyInput KeyInput;

        public override ManagerType ManagerType { get { return ManagerType.Input; } }

        public override void Init()
        {
        }
    }
}
