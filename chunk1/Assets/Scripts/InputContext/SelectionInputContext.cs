using Assets.Scripts.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.InputContext
{
	public class SelectionInputContext : InputContextBase
	{
		private SelectionManager _selectionManager;
		private InputManager _inputManager;

		public SelectionInputContext(ManagerProvider provider)
			: base(provider)
		{
			_inputManager = provider.InputManager;
			_selectionManager = provider.SelectionManager;
		}

		public override void Enable()
		{
			_inputManager.SelectionRect.OnRectFinish += OnRectFinish;
		}

		public override void Disable()
		{
			_inputManager.SelectionRect.OnRectFinish -= OnRectFinish;
		}

		private void OnRectFinish(Vector3 start, Vector3 finish)
		{
			_selectionManager.ProcessSelection(start, finish);
		}
	}
}
