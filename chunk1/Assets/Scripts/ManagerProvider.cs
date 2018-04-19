using System.Collections.Generic;
using Assets.Scripts.Commands;
using Assets.Scripts.Core;
using Assets.Scripts.Input;
using Assets.Scripts.InputContext;
using UnityEngine;

namespace Assets.Scripts
{
    public class ManagerProvider : SingletonMonoBehaviour<ManagerProvider>
	{
		public GameSettings GameSettings;
		public UnitManager UnitManager;
		public InputManager InputManager;
		public SelectionManager SelectionManager;
		public CommandManager CommandManager;
		public TimeManager TimeManager;
        public InputContextManager InputContextManager;

        private Dictionary<ManagerType, ManagerBase> _managers = new Dictionary<ManagerType, ManagerBase>();

        protected override void Awake()
        {
            base.Awake();

            GameSettings = GameSettings.Instance;

            UnitManager = GameObject.FindObjectOfType<UnitManager>();
            InputManager = GameObject.FindObjectOfType<InputManager>();
            SelectionManager = GameObject.FindObjectOfType<SelectionManager>();
            CommandManager = GameObject.FindObjectOfType<CommandManager>();
            TimeManager = GameObject.FindObjectOfType<TimeManager>();
            InputContextManager = GameObject.FindObjectOfType<InputContextManager>();

            _managers[UnitManager.ManagerType] = UnitManager;
            _managers[InputManager.ManagerType] = InputManager;
            _managers[SelectionManager.ManagerType] = SelectionManager;
            _managers[CommandManager.ManagerType] = CommandManager;
            _managers[TimeManager.ManagerType] = TimeManager;
            _managers[InputContextManager.ManagerType] = InputContextManager;

            TimeManager.Init();
            UnitManager.Init();
            InputManager.Init();
            SelectionManager.Init();
            CommandManager.Init();
            InputContextManager.Init();
        }
	}
}
