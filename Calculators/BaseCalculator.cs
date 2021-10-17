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
        public CombatState State { get; set; }
        public MonitoredFight Fight { get; set; }

        internal IPandaLogger _logger;
        internal IStatsReporter _statsReporting;

        public BaseCalculator(IPandaLogger logger, IStatsReporter reporter, CombatState state, MonitoredFight fight)
        {
            _logger = logger;
            _statsReporting = reporter;
            State = state;
            Fight = fight;
        }

        public virtual void CalculateEvent(ICombatEvent combatEvent)
        {
            
        }

        public virtual void FinalizeFight()
        {
            
        }

        public virtual void StartFight()
        {
            
        }
    }
}
