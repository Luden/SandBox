using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Core
{
	public class SingletonMonobeh<T> : MonoBehaviour where T: new()
	{
		private static T _instance;
		public static T Instance { get { return _instance; } }

		protected virtual void Awake()
		{
			_instance = new T();
		}
	}
}
