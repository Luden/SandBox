using System;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    public class CommandBase
	{
		public Action<CommandBase> OnCancel;
		public Action<CommandBase> OnFinish;

		public CommandState State { get; private set; }

		public virtual CommandType GetCommandType() { return CommandType.None; }

		public virtual void Init()
		{
			State = CommandState.Inited;
		}

		public virtual void Start()
		{
			State = CommandState.Started;
		}

		public virtual void Init(Vector3 target)
		{
			Init();
		}

		public virtual void Start(Unit unit)
		{
			Start();
		}

		public virtual void Update(Unit unit)
		{
		}

		public virtual void Finish()
		{
			State = CommandState.Finished;
			if (OnFinish != null)
				OnFinish(this);
		}

		public virtual void Cancel()
		{
			State = CommandState.Canceled;
			if (OnCancel != null)
				OnCancel(this);
		}

		public void SetPooled()
		{
			State = CommandState.Pooled;
		}
	}
}
