﻿using System;
using System.Collections.Generic;
using Assets.Scripts.Commands;
using Assets.Scripts.Core;
using Assets.Scripts.Movement;
using Assets.Scripts.Parts;
using Assets.Scripts.Perception;
using Assets.Scripts.Players;
using Assets.Scripts.Weapons;

namespace Assets.Scripts.Units
{
    public class Unit
    {
        public int Id;

        public IUnitObject UnitObject;
        public Selectable Selectable;
        public CommandProcessor CommandProcessor;
        public Navigation Navigation;
        public Player Player;
        public Visibility Visibility;
        public Following Following;
        public UnitTargeting Targeting;
        public Arsenal Arsenal;
        public Hull Hull;
        public Partset Partset;

        public Action<Unit> OnDeath;

        public Unit(int id, IUnitObject unitObject, Faction startingFaction)
        {
            Id = id;
            UnitObject = unitObject;

            var provider = ManagerProvider.Instance;
            Player = provider.PlayerManager.GetPlayer(startingFaction);
            CommandProcessor = new CommandProcessor(provider.CommandManager.CommandFactory, provider.TimeManager, provider.GameSettings.UnitCommandsUpdatePeriod, this);
            Partset = new Partset();
            Navigation = new Navigation(UnitObject);
            Following = new Following(Navigation, provider.TimeManager);
            Targeting = new UnitTargeting(Player.Faction);
            Arsenal = new Arsenal(Id, Navigation, Targeting, Following, Partset, provider.ShotsManager, provider.TimeManager);
            Selectable = new Selectable();
            Visibility = new Visibility(Player.Faction);
            Hull = new Hull();
            Hull.OnDeath += Die;
        }

        public void Die()
        {
            CommandProcessor.Stop();
            Navigation.Stop();
            Following.Stop();
            Arsenal.Deinit();
            if (OnDeath != null)
                OnDeath(this);
        }

        public void Stop()
        {
            CommandProcessor.Stop();
        }
    }
}