using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.InputContext
{
	public class InputContextBase
	{
		public Action<InputContextType, InputContextBase> WantEnableContext;
		public Action<InputContextType, InputContextBase> WantDisableContext;

		protected ManagerProvider _provider;
		protected bool _enabled;

		public InputContextBase(ManagerProvider provider)
		{
			_provider = provider;
		}

		public virtual void Init()
		{
		}

		public bool IsEnabled()
		{
			return _enabled;
		}

		public virtual void Enable()
		{
			_enabled = true;
		}

		public virtual void Disable()
		{
			_enabled = false;
		}

		protected void EnableContext(InputContextType type)
		{
			if (WantEnableContext != null)
				WantEnableContext(type, this);
		}

		protected void DisableContext(InputContextType type)
		{
			if (WantDisableContext != null)
				WantDisableContext(type, this);
		}

		protected void SwitchContext(InputContextType type, bool state)
		{
			if (WantDisableContext != null)
				WantDisableContext(type, this);
		}
	}
}
