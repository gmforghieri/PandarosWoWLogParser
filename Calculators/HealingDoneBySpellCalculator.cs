using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PandarosWoWLogParser.Calculators
{
    public class HealingDoneBySpellCalculator : ICalculator
    {
        Dictionary<string, long> _healingDoneBySpellTotal = new Dictionary<string, long>();
        Dictionary<string, Dictionary<string, long>> _healingSpellByPlayer = new Dictionary<string, Dictionary<string, long>>();
        Dictionary<string, long> _healingDoneBySpellsFight = new Dictionary<string, long>();
        Dictionary<string, Dictionary<string, long>> _healingSpellByPlayerFight = new Dictionary<string, Dictionary<string, long>>();

        public List<string> ApplicableEvents { get; set; } = new List<string>()
        {
            LogEvents.SPELL_HEAL,
            LogEvents.SPELL_PERIODIC_HEAL
        };

        IPandaLogger _logger;

        public HealingDoneBySpellCalculator(IPandaLogger logger)
        {
            _logger = logger;
        }

        public void CalculateEvent(ICombatEvent combatEvent, CombatState state)
        {
            var damage = (ISpellHeal)combatEvent;
            var spell = (ISpellBase)combatEvent;

            if (combatEvent.SourceFlags.GetFlagType() == UnitFlags.FlagType.Player && combatEvent.SourceFlags.GetFlagType() == UnitFlags.FlagType.Player)
            {
                _healingDoneBySpellTotal.AddValue(spell.SpellName, damage.Overhealing);
                _healingSpellByPlayer.AddValue(combatEvent.SourceName, spell.SpellName, damage.Overhealing);

                if (state.InFight)
                {
                    _healingDoneBySpellsFight.AddValue(spell.SpellName, damage.Overhealing);
                    _healingSpellByPlayerFight.AddValue(combatEvent.SourceName, spell.SpellName, damage.Overhealing);
                }
            }
        }

        public void FinalizeCalculations(CombatState state)
        {
            int i = 0;
            var total = _healingDoneBySpellTotal.Sum(kvp => kvp.Value);

            _logger.Log("---------------------------------------------");
            _logger.Log("Healing By Spell Raid wide");
            _logger.Log("---------------------------------------------");
            foreach (var person in _healingDoneBySpellTotal.OrderBy(i => i.Value).Reverse())
            {
                i++;
                _logger.Log($"{i}. {person.Key}: {person.Value.ToString("N")} ({Math.Round(person.Value / (double)total, 2) * 100 }%)");
            }
        }

        public void FinalizeFight(MonitoredFight fight, CombatState state)
        {
            int i = 0;
            var ts = fight.FightEnd.Subtract(fight.FightStart);
            var total = _healingDoneBySpellTotal.Sum(kvp => kvp.Value);
            _logger.Log("---------------------------------------------");
            _logger.Log($"Healing By Spell Raid wide {fight.CurrentZone.ZoneName}: {fight.BossName}");
            _logger.Log("---------------------------------------------");
            foreach (var person in _healingDoneBySpellsFight.OrderBy(i => i.Value).Reverse())
            {
                i++;
                _logger.Log($"{i}. {person.Key}: {person.Value.ToString("N")} ({Math.Round(person.Value / (double)total, 2) * 100 }%)");
            }
            _logger.Log("---------------------------------------------");
            _logger.Log($"HPS By Spell Raid wide {fight.CurrentZone.ZoneName}: {fight.BossName}");
            _logger.Log("---------------------------------------------");
            foreach (var person in _healingDoneBySpellsFight.OrderBy(i => i.Value).Reverse())
            {
                i++;
                _logger.Log($"{i}. {person.Key}: {(person.Value / ts.TotalSeconds).ToString("N")}  ({Math.Round(person.Value / (double)total, 2) * 100 }%)");
            }
            _logger.Log("---------------------------------------------");
        }

        public void StartFight(MonitoredFight fight, CombatState state)
        {
            _healingDoneBySpellsFight.Clear();
            _healingSpellByPlayerFight.Clear();

        }
    }
}
