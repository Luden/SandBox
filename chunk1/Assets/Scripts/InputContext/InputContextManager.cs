using System.Collections.Generic;
using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.InputContext
{
    public class InputContextManager : ManagerBase
	{
		ManagerProvider _provider;

		SortedDictionary<InputContextType, InputContextBase> _contexts;

        public override ManagerType ManagerType { get { return ManagerType.InputContext; } }

        public override void Init()
		{
            _provider = ManagerProvider.Instance;
            _contexts = new SortedDictionary<InputContextType, InputContextBase>();
			_contexts[InputContextType.Selection] = new SelectionInputContext(_provider);
			_contexts[InputContextType.Movement] = new MovementInputContext(_provider);
			SubscribeContextNeeds();

			_contexts[InputContextType.Selection].Enable();
		}

		private void SubscribeContextNeeds()
		{
			foreach (var context in _contexts.Values)
			{
				context.WantDisableContext += ContextDisableHandler;
				context.WantEnableContext += ContextEnableHandler;
			}
		}

		private void ContextEnableHandler(InputContextType type, InputContextBase ctx)
		{
			var context = _contexts[type];
			if (!context.IsEnabled())
				context.Enable();
		}

		private void ContextDisableHandler(InputContextType type, InputContextBase ctx)
		{
			var context = _contexts[type];
			if (context.IsEnabled())
				_contexts[type].Disable();
		}
	}
}