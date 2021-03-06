﻿using System.IO;
using UnityEngine;

public class GameSettings
{
	const string FileName = "settings.json";
	private static GameSettings _instance = GameSettings.Load();
	public static GameSettings Instance { get { return _instance; } }

	public float CameraSpeed = 1f;
	public float UnitCommandsUpdatePeriod = 0.1f;

	public void Save()
	{
		File.WriteAllText(FileName, JsonUtility.ToJson(this, true));
	}

	public static GameSettings Load()
	{
		try
		{
			return JsonUtility.FromJson<GameSettings>(File.ReadAllText(FileName));
		}
		catch
		{
			return new GameSettings();
		}
	}
}