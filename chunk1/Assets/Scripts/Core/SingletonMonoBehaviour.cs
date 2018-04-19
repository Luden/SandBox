using UnityEngine;

namespace Assets.Scripts.Core
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
		public static T Instance { get; private set; }

		protected virtual void Awake()
		{
            Instance = (T)this;
		}
	}
}
