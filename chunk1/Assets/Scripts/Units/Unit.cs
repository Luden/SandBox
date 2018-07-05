using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Commands;
using Assets.Scripts.Movement;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public ISelectable Selectable;
	public CommandProcessor CommandProcessor;
    public Navigation Navigation;

    void Start()
	{
        Navigation = GetComponent<Navigation>();
        Selectable = GetComponent<Selectable>();

        var provider = ManagerProvider.Instance;
		CommandProcessor = new CommandProcessor(provider.CommandManager.CommandFactory, provider.TimeManager, provider.GameSettings.UnitCommandsUpdatePeriod, this);
	}

	public void Stop()
	{
		CommandProcessor.Stop();
	}
}
