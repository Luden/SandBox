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
	public NavMeshAgent NavMeshAgent;
    public NavMeshObstacle NavMeshObstacle;
    public Neighbourhood Neighbourhood;

    void Start()
	{
        Selectable = GetComponent<Selectable>();
		NavMeshAgent = GetComponent<NavMeshAgent>();
        NavMeshObstacle = GetComponent<NavMeshObstacle>();
        Neighbourhood = GetComponent<Neighbourhood>();

        var provider = ManagerProvider.Instance;
		CommandProcessor = new CommandProcessor(provider.CommandManager.CommandFactory, provider.TimeManager, provider.GameSettings.UnitCommandsUpdatePeriod, this);
	}

	public void Stop()
	{
		CommandProcessor.Stop();
	}
}
