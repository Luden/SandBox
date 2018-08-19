using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts
{
    public class RegularUpdate
	{
		public Action<float> Update;
		public float Period;
		public float LastUpdate;
        public bool IsOnce;
        public bool IsCancelled;
    }

	public class TimeManager : ManagerBase
	{
		private List<RegularUpdate> _tasks = new List<RegularUpdate>();
		private List<RegularUpdate> _taskPool = new List<RegularUpdate>();
		private List<RegularUpdate> _taskToRemove = new List<RegularUpdate>();

        public override ManagerType ManagerType { get { return ManagerType.Time; } }

        private RegularUpdate GetOrCreate()
		{
			if (_taskPool.Count == 0)
				return new RegularUpdate();

			var result = _taskPool.First();
			_taskPool.RemoveAt(0);
			return result;
		}

		public void StartUpdate(ref RegularUpdate task, Action<float> update, float period)
		{
            if (task != null)
            {
                if (task.Update == update)
                    return;
                StopUpdate(ref task);
            }
			task = GetOrCreate();
			task.Update = update;
			task.Period = period;
			task.LastUpdate = Time.time;
            task.IsOnce = false;
            task.IsCancelled = false;
            _tasks.Add(task);
		}

        //public RegularUpdate LateUpdateOnce(Action<float> update)
        //{
        //    var task = StartUpdate(update, Time.fixedDeltaTime + Mathf.Epsilon);
        //    task.IsOnce = true;
        //    return task;
        //}

        public void StopUpdate(ref RegularUpdate update)
        {
            StopUpdate(update);
            update = null;
        }

        private void StopUpdate(RegularUpdate update)
		{
            if (update == null)
                return;

            update.IsCancelled = true;
            _taskToRemove.Add(update);
        }

		private void FixedUpdate()
		{
			float time = Time.time;

			if (_taskToRemove.Count > 0)
			{
				foreach (var task in _taskToRemove)
				{
					_tasks.Remove(task);
					_taskPool.Add(task);
				}
				_taskToRemove.Clear();
			}

			foreach (var task in _tasks)
			{
				if (!task.IsCancelled && task.LastUpdate + task.Period < time)
				{
					task.Update(time - task.LastUpdate);
					task.LastUpdate = time;
                    if (task.IsOnce)
                        StopUpdate(task);
				}
			}
		}
	}
}
