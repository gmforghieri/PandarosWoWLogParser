using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PandarosWoWLogParser.Calculators
{
    public class SpellsCastCalculator : BaseCalculator
    {
        Dictionary<string, Dictionary<string, long>> _spellsCast = new Dictionary<string, Dictionary<string, long>>();

        public SpellsCastCalculator(IPandaLogger logger, IStatsReporter reporter, CombatState state, MonitoredFight fight) : base(logger, reporter, state, fight)
        {
            ApplicableEvents = new List<string>()
            {
                LogEvents.SPELL_CAST_SUCCESS
            };
        }

        public override void CalculateEvent(ICombatEvent combatEvent)
        {
            if (combatEvent.SourceFlags.GetFlagType != UnitFlags.FlagType.Player)
                return;
            var spell = (ISpellBase)combatEvent;

            _spellsCast.AddValue(combatEvent.SourceName, spell.SpellName, 1);
        }

        public override void FinalizeFight()
        {
            _statsReporting.Report(_spellsCast, "Spells Cast", Fight, State);
        }

        public override void StartFight()
        {
            
        }
    }
}
