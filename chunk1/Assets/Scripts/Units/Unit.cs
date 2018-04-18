using Assets.Scripts;
using Assets.Scripts.Commands;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public ISelectable Selectable;
	public CommandProcessor CommandProcessor;
	public NavMeshAgent NavMeshAgent;

	void Start()
	{
        Selectable = GetComponent<Selectable>();
		NavMeshAgent = GetComponent<NavMeshAgent>();

		var provider = ManagerProvider.Instance;
		CommandProcessor = new CommandProcessor(provider.CommandManager.CommandFactory, provider.TimeManager, provider.GameSettings.UnitCommandsUpdatePeriod, this);
	}

	public void Stop()
	{
		CommandProcessor.Stop();
	}
}
