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
		public UnitsManager UnitManager;
		public InputManager InputManager;
		public SelectionManager SelectionManager;
		public CommandManager CommandManager;
		public TimeManager TimeManager;
        public InputContextManager InputContextManager;

        protected override void Awake()
        {
            base.Awake();

            if (GameSettings == null)
                GameSettings = GameSettings.Instance;
            if (UnitManager == null)
                UnitManager = GameObject.FindObjectOfType<UnitsManager>();
            if (InputManager == null)
                InputManager = GameObject.FindObjectOfType<InputManager>();
            if (SelectionManager == null)
                SelectionManager = GameObject.FindObjectOfType<SelectionManager>();
            if (CommandManager == null)
                CommandManager = GameObject.FindObjectOfType<CommandManager>();
            if (TimeManager == null)
                TimeManager = GameObject.FindObjectOfType<TimeManager>();
            if (InputContextManager == null)
                InputContextManager = GameObject.FindObjectOfType<InputContextManager>();
        }
	}
}
