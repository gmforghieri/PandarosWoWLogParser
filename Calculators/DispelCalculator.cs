using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PandarosWoWLogParser.Calculators
{
    public class DispelCalculator : BaseCalculator
    {
        // Player, Dispell Spell, Dispelled, count
        Dictionary<string, Dictionary<string, Dictionary<string, long>>> _Dispells = new Dictionary<string, Dictionary<string, Dictionary<string, long>>>();

        public DispelCalculator(IPandaLogger logger, IStatsReporter reporter, ICombatState state, MonitoredFight fight) : base(logger, reporter, state, fight)
        {
            ApplicableEvents = new List<string>()
            {
                LogEvents.SPELL_DISPEL
            };
        }

        public override void CalculateEvent(ICombatEvent combatEvent)
        {
            if (combatEvent.SourceFlags.GetFlagType != UnitFlags.FlagType.Player && combatEvent.SourceFlags.GetController != UnitFlags.Controller.Player)
                return;
            var spell = (SpellDispel)combatEvent;

            if (State.TryGetOwnerName(combatEvent, out string owner))
                _Dispells.AddValue(owner, spell.SpellName, spell.ExtraSpellName, 1);
            else
                _Dispells.AddValue(combatEvent.SourceName, spell.SpellName, spell.ExtraSpellName, 1);
        }

        public override void FinalizeFight()
        {
            _statsReporting.Report(_Dispells, "Dispells", Fight, State);
        }

        public override void StartFight()
        {
            
        }
    }
}
