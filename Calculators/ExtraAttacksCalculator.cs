using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PandarosWoWLogParser.Calculators
{
    public class ExtraAttacksCalculator : BaseCalculator
    {
        Dictionary<string, Dictionary<string, long>> _extraAttackCount = new Dictionary<string, Dictionary<string, long>>();

        public ExtraAttacksCalculator(IPandaLogger logger, IStatsReporter reporter, CombatState state, MonitoredFight fight) : base(logger, reporter, state, fight)
        {
            ApplicableEvents = new List<string>()
            {
                LogEvents.SPELL_EXTRA_ATTACKS
            };
        }

        public override void CalculateEvent(ICombatEvent combatEvent)
        {
            if (combatEvent.SourceFlags.GetFlagType != UnitFlags.FlagType.Player)
                return;

            var spell = (SpellExtraAttacks)combatEvent;

            _extraAttackCount.AddValue(combatEvent.SourceName, spell.SpellName, spell.Amount);
        }

        public override void FinalizeFight()
        {
            _statsReporting.Report(_extraAttackCount, "Extra Attacks", Fight, State);
        }

        public override void StartFight()
        {
            
        }
    }
}
