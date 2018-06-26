using Assets.Scripts.Input;
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
            _inputManager.SelectionRect.OnRectUpdate += OnRectUpdate;
            _inputManager.SelectionRect.OnIdle += OnRectIdle;
        }

		public override void Disable()
		{
			_inputManager.SelectionRect.OnRectFinish -= OnRectFinish;
            _inputManager.SelectionRect.OnRectUpdate -= OnRectUpdate;
            _inputManager.SelectionRect.OnIdle -= OnRectIdle;
        }

        private void OnRectIdle(Vector3 position)
        {
            _selectionManager.ProcessPreselection(position);
        }

        private void OnRectUpdate(Vector3 start, Vector3 finish)
        {
            _selectionManager.ProcessPreselection(start, finish);
        }

        private void OnRectFinish(Vector3 start, Vector3 finish)
		{
			_selectionManager.ProcessSelection(start, finish);
		}
	}
}
