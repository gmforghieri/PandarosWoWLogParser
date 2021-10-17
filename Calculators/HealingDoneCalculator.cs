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

        Dictionary<string, long> _healingDoneByPlayersFight = new Dictionary<string, long>();
        Dictionary<string, long> _overHealingDoneByPlayersFight = new Dictionary<string, long>();

        public HealingDoneCalculator(IPandaLogger logger, IStatsReporting reporter) : base(logger, reporter)
        {
            ApplicableEvents = new List<string>()
        {
            LogEvents.SPELL_HEAL,
            LogEvents.SPELL_PERIODIC_HEAL
        };
        }

        public override void CalculateEvent(ICombatEvent combatEvent, CombatState state)
        {
            var damage = (ISpellHeal)combatEvent;

            if (combatEvent.SourceFlags.GetFlagType() == UnitFlags.FlagType.Player && combatEvent.SourceFlags.GetFlagType() == UnitFlags.FlagType.Player)
            {
                _healingDoneByPlayersTotal.AddValue(combatEvent.SourceName, damage.HealAmount);
                _overHealingDoneByPlayersTotal.AddValue(combatEvent.SourceName, damage.Overhealing);

                if (state.InFight)
                {
                    _healingDoneByPlayersFight.AddValue(combatEvent.SourceName, damage.HealAmount);
                    _overHealingDoneByPlayersFight.AddValue(combatEvent.SourceName, damage.Overhealing);
                }
            }
        }

        public override void FinalizeCalculations(CombatState state)
        {
            _statsReporting.Report(_healingDoneByPlayersTotal, "Healing Rankings Total", state);
            _statsReporting.Report(_overHealingDoneByPlayersTotal, "Overhealed Rankings Total", state);
        }

        public override void FinalizeFight(MonitoredFight fight, CombatState state)
        {
            _statsReporting.Report(_healingDoneByPlayersFight, "Healing Rankings", fight, state);
            _statsReporting.Report(_overHealingDoneByPlayersFight, "Overhealed Rankings", fight, state);
            _statsReporting.ReportPerSecondNumbers(_healingDoneByPlayersFight, "HPS Rankings", fight, state);
        }

        public override void StartFight(MonitoredFight fight, CombatState state)
        {
            _overHealingDoneByPlayersFight.Clear();
            _healingDoneByPlayersFight.Clear();
        }
    }
}
