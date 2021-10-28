using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PandarosWoWLogParser.Calculators
{
    public class HealingDoneBySpellCalculator : BaseCalculator
    {
        Dictionary<string, long> _healingDoneBySpellTotal = new Dictionary<string, long>();
        Dictionary<string, Dictionary<string, long>> _healingSpellByPlayer = new Dictionary<string, Dictionary<string, long>>();

        public HealingDoneBySpellCalculator(IPandaLogger logger, IStatsReporter reporter, ICombatState state, MonitoredFight fight) : base(logger, reporter, state, fight)
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
            var spell = (ISpell)combatEvent;

            if (combatEvent.SourceFlags.GetFlagType == UnitFlags.FlagType.Player)
            {
                _healingDoneBySpellTotal.AddValue(spell.SpellName, damage.HealAmount);
                _healingSpellByPlayer.AddValue(combatEvent.SourceName, spell.SpellName, damage.HealAmount);
            } else if (State.TryGetSourceOwnerName(combatEvent, out var owner))
            {
                _healingDoneBySpellTotal.AddValue(spell.SpellName, damage.HealAmount);
                _healingSpellByPlayer.AddValue(owner, spell.SpellName, damage.HealAmount);
            }
        }

        public override void FinalizeFight()
        {
            _statsReporting.Report(_healingDoneBySpellTotal, "Healing By Spell Raid wide", Fight, State);
            _statsReporting.ReportPerSecondNumbers(_healingDoneBySpellTotal, "HPS By Spell Raid wide", Fight, State);
            _statsReporting.Report(_healingSpellByPlayer, "Healing Spell by Player", Fight, State);
        }

        public override void StartFight()
        {

        }
    }
}
