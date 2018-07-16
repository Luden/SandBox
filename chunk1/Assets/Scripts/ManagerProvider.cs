using System.Collections.Generic;
using Assets.Scripts.Commands;
using Assets.Scripts.Core;
using Assets.Scripts.Input;
using Assets.Scripts.InputContext;
using Assets.Scripts.Player;
using Assets.Scripts.Visibility;
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
        public PlayerManager PlayerManager;
        public VisibilityManager VisibilityManager;

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
            PlayerManager = GameObject.FindObjectOfType<PlayerManager>();
            VisibilityManager = GameObject.FindObjectOfType<VisibilityManager>();

            _managers[UnitManager.ManagerType] = UnitManager;
            _managers[InputManager.ManagerType] = InputManager;
            _managers[SelectionManager.ManagerType] = SelectionManager;
            _managers[CommandManager.ManagerType] = CommandManager;
            _managers[TimeManager.ManagerType] = TimeManager;
            _managers[InputContextManager.ManagerType] = InputContextManager;
            _managers[PlayerManager.ManagerType] = PlayerManager;
            _managers[VisibilityManager.ManagerType] = VisibilityManager;

            TimeManager.Init();
            UnitManager.Init();
            InputManager.Init();
            SelectionManager.Init();
            CommandManager.Init();
            InputContextManager.Init();
            PlayerManager.Init();
            VisibilityManager.Init();
        }
	}
}
