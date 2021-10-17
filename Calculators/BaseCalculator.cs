using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Calculators
{
    public abstract class BaseCalculator : ICalculator
    {
        public List<string> ApplicableEvents { get; set; }

        internal IPandaLogger _logger;
        internal IStatsReporting _statsReporting;

        public BaseCalculator(IPandaLogger logger, IStatsReporting reporter)
        {
            _logger = logger;
            _statsReporting = reporter;
        }

        public virtual void CalculateEvent(ICombatEvent combatEvent, CombatState state)
        {
            
        }

        public virtual void FinalizeCalculations(CombatState state)
        {
            
        }

        public virtual void FinalizeFight(MonitoredFight fight, CombatState state)
        {
            
        }

        public virtual void StartFight(MonitoredFight fight, CombatState state)
        {
            
        }
    }
}
