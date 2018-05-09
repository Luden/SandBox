using System;
using Assets.Scripts.Core;
using Assets.Scripts.Formations;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    public class CommandBase : IKeyProvider<CommandType>
    {
        public Action<CommandBase> OnCancel;
        public Action<CommandBase> OnFinish;
        public Action<CommandBase> OnStop;

        public Unit Unit { get; private set; }
        public Formation Formation { get; private set; }

        public CommandState State { get; private set; }

		public virtual CommandType GetKey() { return CommandType.None; }

		public virtual void Init()
		{
			State = CommandState.Inited;
		}

		public virtual void Init(Vector3 target, Formation formation)
		{
            Formation = formation;
            Init();
		}

		public virtual void Start(Unit unit)
		{
            State = CommandState.Started;
            Unit = unit;
		}

		public virtual void Update(float dt)
		{
		}

		public virtual void Finish()
		{
            Stop();
            State = CommandState.Finished;
			if (OnFinish != null)
				OnFinish(this);
		}

		public virtual void Cancel()
		{
            Stop();
            State = CommandState.Canceled;
			if (OnCancel != null)
				OnCancel(this);
		}

		public void SetPooled()
		{
			State = CommandState.Pooled;
		}

        protected virtual void Stop()
        {
            if (OnStop != null)
                OnStop(this);
        }
	}
}
