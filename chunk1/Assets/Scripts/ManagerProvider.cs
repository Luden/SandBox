using Assets.Scripts.Commands;
using Assets.Scripts.Core;
using Assets.Scripts.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
	public class ManagerProvider : SingletonMonobeh<ManagerProvider>
	{
		public GameSettings GameSettings;
		public UnitsManager UnitManager;
		public InputManager InputManager;
		public SelectionManager SelectionManager;
		public CommandManager CommandManager;
		public TimeManager TimeManager;
	}
}
