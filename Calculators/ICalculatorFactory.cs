using System;
using System.Collections.Generic;
using System.Text;
using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;

namespace PandarosWoWLogParser.Calculators
{
    public interface ICalculatorFactory
    {
        public Dictionary<string, List<ICalculator>> Calculators { get; set; }

        public void CalculateEvent(CombatEventBase combatEvent);

        public void FinalizeCalculations();

        public void StartFight(MonitoredFight fight);

        public void FinalizeFight(MonitoredFight fight);
    }
}
