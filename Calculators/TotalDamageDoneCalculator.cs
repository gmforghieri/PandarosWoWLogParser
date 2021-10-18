using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PandarosWoWLogParser.Calculators
{
    public class TotalDamageDoneCalculator : BaseCalculator
    {
        Dictionary<string, long> _damageDoneByPlayersTotal = new Dictionary<string, long>();

        public TotalDamageDoneCalculator(IPandaLogger logger, IStatsReporter reporter, CombatState state, MonitoredFight fight) : base(logger, reporter, state, fight)
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
            if (combatEvent.SourceFlags.GetFlagType != UnitFlags.FlagType.Player)
                return;

            var damage = (IDamage)combatEvent;

            _damageDoneByPlayersTotal.AddValue(combatEvent.SourceName, damage.Damage);

            if (State.TryGetOwnerName(combatEvent, out var owner))
            {
                _damageDoneByPlayersTotal.AddValue(owner, damage.Damage);
            }
        }

        public override void FinalizeFight()
        {
            _statsReporting.Report(_damageDoneByPlayersTotal, "Damage Rankings", Fight, State);
            _statsReporting.ReportPerSecondNumbers(_damageDoneByPlayersTotal, "DPS Rankings", Fight, State);
        }

        public override void StartFight()
        {
            
        }
    }
}
