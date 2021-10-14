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

        public void CalculateEvent(ICombatEvent combatEvent, CombatState state);

        public void FinalizeCalculations(CombatState state);

        public void StartFight(MonitoredFight fight, CombatState state);

        public void FinalizeFight(MonitoredFight fight, CombatState state);
    }
}
