using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PandarosWoWLogParser.Calculators
{
    public class aaFightStartCalculator : BaseCalculator
    {
        string initiator;

        public aaFightStartCalculator(IPandaLogger logger, IStatsReporter reporter, ICombatState state, MonitoredFight fight) : base(logger, reporter, state, fight)
        {
            ApplicableEvents = FightMonitorFactory.CombatEventsTriggerInFight;
        }

        public override void CalculateEvent(ICombatEvent combatEvent)
        {
            if (!string.IsNullOrEmpty(initiator) || (!combatEvent.DestFlags.IsNPC && !combatEvent.SourceFlags.IsNPC))
                return;

            if (combatEvent.SourceFlags.GetFlagType == UnitFlags.FlagType.Player)
                initiator = combatEvent.SourceName;
            else if (combatEvent.DestFlags.GetFlagType == UnitFlags.FlagType.Player)
                initiator = combatEvent.DestName;
            else if(State.TryGetSourceOwnerName(combatEvent, out var owner))
                initiator = owner;
            else if (State.TryGetDestOwnerName(combatEvent, out owner))
                initiator = owner;
        }

        public override void FinalizeFight()
        {
            _logger.Log("---------------------------------------------");
            _logger.Log($"Person who started the fight for {Fight.BossName}");
            _logger.Log("---------------------------------------------");
            _logger.Log("~~~~~~~" + initiator + "~~~~~~~");

        }

        public override void StartFight()
        {
            
        }
    }
}
