using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Commands;
using Assets.Scripts.Core;
using Assets.Scripts.Movement;
using Assets.Scripts.Perception;
using Assets.Scripts.Players;
using Assets.Scripts.Weapons;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Units
{
    public class Unit : MonoBehaviour
    {
        public Selectable Selectable;
        public CommandProcessor CommandProcessor;
        public Navigation Navigation;
        public Player Player;
        public Visibility Visibility;
        public Following Following;
        public Targeting Targeting;
        public Arsenal Arsenal;
        public Hull Hull;

        [SerializeField]
        private Faction _startingFaction;

        void Start()
        {
            Navigation = GetComponent<Navigation>();
            

            var provider = ManagerProvider.Instance;
            Player = provider.PlayerManager.GetPlayer(_startingFaction);
            CommandProcessor = new CommandProcessor(provider.CommandManager.CommandFactory, provider.TimeManager, provider.GameSettings.UnitCommandsUpdatePeriod, this);
            Following = new Following(Navigation, provider.TimeManager);
            Targeting = new Targeting(Navigation, Player.Faction);
            Arsenal = new Arsenal(Navigation, Targeting, Following, provider.ShotsManager, provider.TimeManager);
            Hull = new Hull();
            Selectable = new Selectable();
            Hull.OnDeath += Die;

            Visibility = new Visibility(Player.Faction);
        }

        public void Die()
        {
            CommandProcessor.Stop();
            Navigation.Stop();
            Following.Stop();
            Arsenal.Stop();

            Destroy(gameObject);
        }

        public void Stop()
        {
            CommandProcessor.Stop();
        }
    }
}