﻿using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.InputContext
{
    public class InputContextManager : MonoBehaviour
	{
		ManagerProvider _provider;

		SortedDictionary<InputContextType, InputContextBase> _contexts;

		void Start()
		{
            _provider = ManagerProvider.Instance;
            Init();
		}

		private void Init()
		{
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