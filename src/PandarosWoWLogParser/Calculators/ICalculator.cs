using System;
using System.Collections.Generic;
using System.Text;
using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;

namespace PandarosWoWLogParser.Calculators
{
    public interface ICalculator
    {
        public List<string> ApplicableEvents { get; set; }
        public ICombatState State { get; set; }
        public MonitoredFight Fight { get; set; }
        public void CalculateEvent(ICombatEvent combatEvent);
        public void StartFight();
        public void FinalizeFight();
    }
}
