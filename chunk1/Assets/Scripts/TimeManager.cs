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

		public RegularUpdate StartUpdate(Action<float> update, float period)
		{
			var task = GetOrCreate();
			task.Update = update;
			task.Period = period;
			task.LastUpdate = Time.time;
            task.IsOnce = false;
            _tasks.Add(task);
			return task;
		}

        public void LateUpdateOnce(Action<float> update)
        {
            var task = StartUpdate(update, Time.fixedDeltaTime + Mathf.Epsilon);
            task.IsOnce = true;
        }

		public void StopUpdate(RegularUpdate update)
		{
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
				if (task.LastUpdate + task.Period < time)
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
