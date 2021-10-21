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

            if (State.TryGetOwnerName(combatEvent, out var owner))
            {
                _healingDoneByPlayersTotal.AddValue(owner, damage.HealAmount);
                _overHealingDoneByPlayersTotal.AddValue(owner, damage.Overhealing);
            }
        }

        public override void FinalizeFight()
        {
            Dictionary<string, long> totalLife = new Dictionary<string, long>();

            foreach (var kvp in _healingDoneByPlayersTotal)
                totalLife.AddValue(kvp.Key, kvp.Value);

            foreach (var kvp in _overHealingDoneByPlayersTotal)
                totalLife.AddValue(kvp.Key, kvp.Value);

            _statsReporting.Report(_healingDoneByPlayersTotal, "Life Healed Rankings", Fight, State);
            _statsReporting.Report(_overHealingDoneByPlayersTotal, "Overhealed Rankings", Fight, State);
            _statsReporting.ReportPerSecondNumbers(_healingDoneByPlayersTotal, "Life Healed HPS Rankings", Fight, State);

            _statsReporting.Report(totalLife, "Healing Output Rankings (Life Healed + Overheal)", Fight, State);
            _statsReporting.ReportPerSecondNumbers(totalLife, "HPS Rankings (Life Healed + Overheal)", Fight, State);
        }

        public override void StartFight()
        {

        }
    }
}
