using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
	public class RegularUpdate
	{
		public Action Update;
		public float Period;
		public float LastUpdate;
	}

	public class TimeManager : MonoBehaviour
	{
		private List<RegularUpdate> _tasks = new List<RegularUpdate>();
		private List<RegularUpdate> _taskPool = new List<RegularUpdate>();
		private List<RegularUpdate> _taskToRemove = new List<RegularUpdate>();

		private RegularUpdate GetOrCreate()
		{
			if (_taskPool.Count == 0)
				return new RegularUpdate();

			var result = _taskPool.First();
			_taskPool.RemoveAt(0);
			return result;
		}

		public RegularUpdate StartUpdate(Action update, float period)
		{
			var task = GetOrCreate();
			task.Update = update;
			task.Period = period;
			task.LastUpdate = Time.time;
			_tasks.Add(task);
			return task;
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
					task.Update();
					task.LastUpdate = time;
				}
			}
		}
	}
}
