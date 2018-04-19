using UnityEngine;

namespace Assets.Scripts.Input
{
    public class KeyInput : MonoBehaviour
	{
        public bool IsShift()
        {
            return UnityEngine.Input.GetKey(KeyCode.LeftShift);
        }
	}
}
