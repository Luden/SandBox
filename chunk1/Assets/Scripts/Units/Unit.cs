using Assets.Scripts;
using Assets.Scripts.Commands;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public ISelectable Selectable;
	public CommandProcessor CommandProcessor;
	public NavMeshAgent NavMeshAgent;
    public NavMeshObstacle NavMeshObstacle;

    void Start()
	{
        Selectable = GetComponent<Selectable>();
		NavMeshAgent = GetComponent<NavMeshAgent>();
        NavMeshAgent.enabled = false;
        NavMeshObstacle = GetComponent<NavMeshObstacle>();

        var provider = ManagerProvider.Instance;
		CommandProcessor = new CommandProcessor(provider.CommandManager.CommandFactory, provider.TimeManager, provider.GameSettings.UnitCommandsUpdatePeriod, this);
	}

	public void Stop()
	{
		CommandProcessor.Stop();
	}
}
