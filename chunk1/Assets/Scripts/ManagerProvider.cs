using System;
using System.Collections.Generic;
using Assets.Scripts.Commands;
using Assets.Scripts.Core;
using Assets.Scripts.Input;
using Assets.Scripts.InputContext;
using Assets.Scripts.Players;
using Assets.Scripts.Shots;
using Assets.Scripts.Perception;
using UnityEngine;
using Assets.Scripts.Units;
using Assets.Scripts.Parts;

namespace Assets.Scripts
{
    public class ManagerProvider : SingletonMonoBehaviour<ManagerProvider>
	{
        [NonSerialized] public UnitObjectManager UnitObjectManager;
        [NonSerialized] public UnitViewManager UnitViewManager;
        [NonSerialized] public PartViewsManager PartViewsManager;
        [NonSerialized] public ShotViewsManager ShotViewsManager;
        [NonSerialized] public InputManager InputManager;

        public GameSettings GameSettings;
		public UnitManager UnitManager;
        public PartsManager PartsManager;
		public SelectionManager SelectionManager;
		public CommandManager CommandManager;
		public TimeManager TimeManager;
        public InputContextManager InputContextManager;
        public PlayerManager PlayerManager;
        public VisibilityManager VisibilityManager;
        public ShotsManager ShotsManager;

        private Dictionary<ManagerType, IManager> _managers = new Dictionary<ManagerType, IManager>();

        protected override void Awake()
        {
            base.Awake();

            GameSettings = GameSettings.Instance;

            UnitObjectManager = GameObject.FindObjectOfType<UnitObjectManager>();
            UnitViewManager = GameObject.FindObjectOfType<UnitViewManager>();
            InputManager = GameObject.FindObjectOfType<InputManager>();
            PartViewsManager = GameObject.FindObjectOfType<PartViewsManager>();
            ShotViewsManager = GameObject.FindObjectOfType<ShotViewsManager>();

            UnitManager = new UnitManager();
            PartsManager = new PartsManager();
            TimeManager = new TimeManager();
            SelectionManager = new SelectionManager();
            CommandManager = new CommandManager();
            InputContextManager = new InputContextManager();
            PlayerManager = new PlayerManager();
            VisibilityManager = new VisibilityManager();
            ShotsManager = new ShotsManager();

            _managers[UnitManager.ManagerType] = UnitManager;
            _managers[UnitViewManager.ManagerType] = UnitViewManager;
            _managers[UnitObjectManager.ManagerType] = UnitObjectManager;
            _managers[InputManager.ManagerType] = InputManager;
            _managers[SelectionManager.ManagerType] = SelectionManager;
            _managers[CommandManager.ManagerType] = CommandManager;
            _managers[TimeManager.ManagerType] = TimeManager;
            _managers[InputContextManager.ManagerType] = InputContextManager;
            _managers[PlayerManager.ManagerType] = PlayerManager;
            _managers[VisibilityManager.ManagerType] = VisibilityManager;
            _managers[ShotsManager.ManagerType] = ShotsManager;
            _managers[ShotViewsManager.ManagerType] = ShotViewsManager;
            _managers[PartsManager.ManagerType] = PartsManager;
            _managers[PartViewsManager.ManagerType] = PartViewsManager;

            foreach (var manager in _managers.Values)
                manager.Init();
        }
	}
}
