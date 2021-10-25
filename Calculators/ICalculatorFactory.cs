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
        public CombatState State { get; set; }
        public void CalculateEvent(ICombatEvent combatEvent);
        public void StartFight();
        public void FinalizeFight();
    }
}
