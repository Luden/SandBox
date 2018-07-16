using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Commands;
using Assets.Scripts.Core;
using Assets.Scripts.Movement;
using Assets.Scripts.Player;
using Assets.Scripts.Visibility;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public ISelectable Selectable;
	public CommandProcessor CommandProcessor;
    public Navigation Navigation;
    public Player Player;
    public Visibility Visibility;
    public Following Following;

    [SerializeField]
    private Faction _startingFaction;

    void Start()
	{
        Navigation = GetComponent<Navigation>();
        Selectable = GetComponent<Selectable>();

        var provider = ManagerProvider.Instance;
        Player = provider.PlayerManager.GetPlayer(_startingFaction);
        CommandProcessor = new CommandProcessor(provider.CommandManager.CommandFactory, provider.TimeManager, provider.GameSettings.UnitCommandsUpdatePeriod, this);
        Following = new Following(provider.TimeManager);

        Visibility = new Visibility(Player.Faction);
    }

	public void Stop()
	{
		CommandProcessor.Stop();
	}
}
