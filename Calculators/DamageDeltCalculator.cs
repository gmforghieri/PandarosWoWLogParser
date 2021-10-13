﻿using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Calculators
{
    public class DamageDeltCalculator : ICalculator
    {
        Dictionary<string, Dictionary<string, long>> _damageDoneByEntityToEntity = new Dictionary<string, Dictionary<string, long>>();

        public List<string> ApplicableEvents { get; set; } = new List<string>()
        {
            LogEvents.SPELL_DAMAGE,
            LogEvents.RANGE_DAMAGE,
            LogEvents.SWING_DAMAGE,
            LogEvents.SPELL_PERIODIC_DAMAGE
        };

        public void CalculateEvent(CombatEventBase combatEvent)
        {
            var damage = (IDamage)combatEvent;
        }

        public void FinalizeCalculations()
        {
            
        }

        public void FinalizeFight(MonitoredFight fight)
        {
            
        }

        public void StartFight(MonitoredFight fight)
        {
            
        }
    }
}