using System.Collections.Generic;
using Assets.Scripts.Commands;
using Assets.Scripts.Input;
using UnityEngine;

namespace Assets.Scripts.InputContext
{
    public class MovementInputContext : InputContextBase
	{
		private InputManager _inputManager;
		private SelectionManager _selectionManager;
		private CommandManager _commandManager;

		public MovementInputContext(ManagerProvider provider)
			: base(provider)
		{
			_inputManager = provider.InputManager;
			_selectionManager = provider.SelectionManager;
			_commandManager = provider.CommandManager;

			_selectionManager.OnSelectionChange += OnSelectionChange;
		}
		
		public override void Enable()
		{
			_inputManager.SelectionRect.OnRectStartRight += OnRightClick;
		}

		public override void Disable()
		{
			_inputManager.SelectionRect.OnRectStartRight -= OnRightClick;
		}

		private void OnSelectionChange(List<ISelectable> selected, List<ISelectable> added, List<ISelectable> removed)
		{
			if (!IsEnabled() && selected.Count > 0)
				Enable();
			else if (IsEnabled() && selected.Count == 0)
				Disable();
		}

		private void OnRightClick(Vector3 start, Vector3 finish)
		{
			var target = _selectionManager.TraceTerrain(start);
			if (target != Vector3.zero)
				_commandManager.Send(CommandType.Move, target, _inputManager.KeyInput.IsShift());
		}
	}
}
