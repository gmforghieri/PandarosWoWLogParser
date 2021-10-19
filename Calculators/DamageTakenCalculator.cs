using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Calculators
{
    public class DamageTakenCalculator : BaseCalculator
    {
        Dictionary<string, Dictionary<string, long>> _damageTakenByEntityFromEntity = new Dictionary<string, Dictionary<string, long>>();

        public DamageTakenCalculator(IPandaLogger logger, IStatsReporter reporter, CombatState state, MonitoredFight fight) : base(logger, reporter, state, fight)
        {
            ApplicableEvents = new List<string>()
                {
                    LogEvents.SPELL_DAMAGE,
                    LogEvents.RANGE_DAMAGE,
                    LogEvents.SWING_DAMAGE,
                    LogEvents.SPELL_PERIODIC_DAMAGE
                };
        }


        public override void CalculateEvent(ICombatEvent combatEvent)
        {
            var damage = (IDamage)combatEvent;

            if (combatEvent.SourceFlags.GetFlagType == UnitFlags.FlagType.Npc && combatEvent.DestFlags.GetFlagType == UnitFlags.FlagType.Player)
            {
                _damageTakenByEntityFromEntity.AddValue(combatEvent.DestName, combatEvent.SourceName, damage.Damage);
            }
        }

        public override void FinalizeFight()
        {
            _statsReporting.Report(_damageTakenByEntityFromEntity, "Damage Taken", Fight, State);
        }

        public override void StartFight()
        {
            
        }
    }
}
