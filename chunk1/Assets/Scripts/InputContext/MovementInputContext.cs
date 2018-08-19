using System.Collections.Generic;
using Assets.Scripts.Commands;
using Assets.Scripts.Input;
using Assets.Scripts.Units;
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
            base.Enable();
		}

		public override void Disable()
		{
			_inputManager.SelectionRect.OnRectStartRight -= OnRightClick;
            base.Disable();
		}

		private void OnSelectionChange(List<Unit> selected, List<Unit> added, List<Unit> removed)
		{
			if (!IsEnabled() && selected.Count > 0)
				Enable();
			else if (IsEnabled() && selected.Count == 0)
				Disable();
		}

		private void OnRightClick(Vector3 start, Vector3 finish)
		{
            if (!_selectionManager.HasSelectedUnits())
                return;

            if (_selectionManager.UnitUnderCursor != null 
                && _selectionManager.UnitUnderCursor.Player.IsEnemy())
            {
                _commandManager.Send(CommandType.Attack, _selectionManager.UnitUnderCursor.Navigation.Position, _selectionManager.UnitUnderCursor, _inputManager.KeyInput.IsShift());
            }
            else
            {
                var target = _selectionManager.TraceTerrain(start);
                if (target != Vector3.zero)
                    _commandManager.Send(CommandType.Move, target, null, _inputManager.KeyInput.IsShift());
            }
		}
	}
}
