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
        Dictionary<string, long> _damageDoneByPlayersFight = new Dictionary<string, long>();

        public TotalDamageDoneCalculator(IPandaLogger logger, IStatsReporting reporter) : base(logger, reporter)
        {
            ApplicableEvents = new List<string>()
            {
                LogEvents.SPELL_DAMAGE,
                LogEvents.RANGE_DAMAGE,
                LogEvents.SWING_DAMAGE,
                LogEvents.SPELL_PERIODIC_DAMAGE
            };
        }

        public override void CalculateEvent(ICombatEvent combatEvent, CombatState state)
        {
            var damage = (IDamage)combatEvent;

            if (combatEvent.SourceFlags.GetFlagType() == UnitFlags.FlagType.Player)
            {
                _damageDoneByPlayersTotal.AddValue(combatEvent.SourceName, damage.Damage);
            }

            if (state.InFight && combatEvent.SourceFlags.GetFlagType() == UnitFlags.FlagType.Player)
            {
                _damageDoneByPlayersFight.AddValue(combatEvent.SourceName, damage.Damage);
            }
        }

        public override void FinalizeCalculations(CombatState state)
        {
            _statsReporting.Report(_damageDoneByPlayersTotal, "Damage Rankings", state);
        }

        public override void FinalizeFight(MonitoredFight fight, CombatState state)
        {
            _statsReporting.Report(_damageDoneByPlayersFight, "Damage Rankings", fight, state);
            _statsReporting.ReportPerSecondNumbers(_damageDoneByPlayersFight, "DPS Rankings", fight, state);
        }

        public override void StartFight(MonitoredFight fight, CombatState state)
        {
            _damageDoneByPlayersFight.Clear();
        }
    }
}
