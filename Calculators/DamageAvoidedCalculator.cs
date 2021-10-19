using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PandarosWoWLogParser.Calculators
{
    public class DamageAvoidedCalculator : BaseCalculator
    {
        Dictionary<string, long> _avoidedDamage = new Dictionary<string, long>();

        public DamageAvoidedCalculator(IPandaLogger logger, IStatsReporter reporter, CombatState state, MonitoredFight fight) : base(logger, reporter, state, fight)
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
            if (combatEvent.DestFlags.GetFlagType != UnitFlags.FlagType.Player)
                return;

            var damage = (IDamage)combatEvent;

            if (damage.Absorbed != 0)
                _avoidedDamage.AddValue(combatEvent.DestName, damage.Absorbed);
        }

        public override void FinalizeFight()
        {
            _statsReporting.Report(_avoidedDamage, "Absorbtion Rankings", Fight, State);
        }

        public override void StartFight()
        {
            
        }
    }
}
