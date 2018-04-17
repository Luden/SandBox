using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.InputContext
{
	public class InputContextResult
	{
		public bool Handled;
		public InputContextType ToEnable;
		public InputContextType ToDisable;
	}
}
