using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PandarosWoWLogParser.Calculators
{
    public class HealingDoneCalculator : BaseCalculator
    {
        Dictionary<string, long> _healingDoneByPlayersTotal = new Dictionary<string, long>();
        Dictionary<string, long> _overHealingDoneByPlayersTotal = new Dictionary<string, long>();

        public HealingDoneCalculator(IPandaLogger logger, IStatsReporter reporter, CombatState state, MonitoredFight fight) : base(logger, reporter, state, fight)
        {
            ApplicableEvents = new List<string>()
        {
            LogEvents.SPELL_HEAL,
            LogEvents.SPELL_PERIODIC_HEAL
        };
        }

        public override void CalculateEvent(ICombatEvent combatEvent)
        {
            var damage = (ISpellHeal)combatEvent;

            if (combatEvent.SourceFlags.GetFlagType == UnitFlags.FlagType.Player)
            {
                _healingDoneByPlayersTotal.AddValue(combatEvent.SourceName, damage.HealAmount);
                _overHealingDoneByPlayersTotal.AddValue(combatEvent.SourceName, damage.Overhealing);
            }
        }

        public override void FinalizeFight()
        {
            _statsReporting.Report(_healingDoneByPlayersTotal, "Healing Rankings", Fight, State);
            _statsReporting.Report(_overHealingDoneByPlayersTotal, "Overhealed Rankings", Fight, State);
            _statsReporting.ReportPerSecondNumbers(_healingDoneByPlayersTotal, "HPS Rankings", Fight, State);
        }

        public override void StartFight()
        {

        }
    }
}
