using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PandarosWoWLogParser.Calculators
{
    public class SpellsCastByNPCCalculator : BaseCalculator
    {
        Dictionary<string, Dictionary<string, List<long>>> _spellsCast = new Dictionary<string, Dictionary<string, List<long>>>();

        public SpellsCastByNPCCalculator(IPandaLogger logger, IStatsReporter reporter, CombatState state, MonitoredFight fight) : base(logger, reporter, state, fight)
        {
            ApplicableEvents = new List<string>()
            {
                LogEvents.SPELL_DAMAGE,
                LogEvents.RANGE_DAMAGE,
                LogEvents.SWING_DAMAGE,
                LogEvents.SPELL_PERIODIC_DAMAGE,
                LogEvents.DAMAGE_SHIELD,
                LogEvents.SPELL_CAST_SUCCESS
            };
        }

        public override void CalculateEvent(ICombatEvent combatEvent)
        {
            if (combatEvent.SourceFlags.GetFlagType == UnitFlags.FlagType.Player || combatEvent.SourceFlags.GetController == UnitFlags.Controller.Player)
                return;

            if (combatEvent is ISpell spell)
            {
                if (combatEvent is IDamage damage)
                {
                    _spellsCast.AddValue(combatEvent.SourceName, spell.SpellName, 0, 1);
                    _spellsCast.AddValue(combatEvent.SourceName, spell.SpellName, 1, damage.Damage);
                }
                else
                {
                    if (_spellsCast.TryGetValue(combatEvent.SourceName, out var casts) && casts.TryGetValue(spell.SpellName, out var castCounts) && castCounts.Count == 1)
                        _spellsCast.AddValue(combatEvent.SourceName, spell.SpellName, 0, 1);
                    else if (!_spellsCast.TryGetValue(combatEvent.SourceName, out var casts2) || !casts2.ContainsKey(spell.SpellName))
                        _spellsCast.AddValue(combatEvent.SourceName, spell.SpellName, 0, 1);
                }
            }
            else if (combatEvent is IDamage damage)
            {
                _spellsCast.AddValue(combatEvent.SourceName, "Swing", 0, 1);
                _spellsCast.AddValue(combatEvent.SourceName, "Swing", 1, damage.Damage);
            }
                
        }

        public override void FinalizeFight()
        {
            List<List<string>> table = new List<List<string>>();
            table.Add(new List<string>()
            {
                "NPC",
                "Attack",
                "Count",
                "Damage"
            });

            var length = new List<int>()
            {
                45,
                30,
                13,
                13
            };

            foreach (var npc in _spellsCast)
            {
                table.Add(new List<string>()
                {
                    npc.Key,
                    string.Empty,
                    string.Empty,
                    string.Empty
                });

                foreach (var attack in npc.Value)
                {
                    if (attack.Value.Count == 2)
                        table.Add(new List<string>()
                        {
                            string.Empty,
                            attack.Key,
                            attack.Value[0].ToString("N"),
                            attack.Value[1].ToString("N")
                        });
                    else
                        table.Add(new List<string>()
                        {
                            string.Empty,
                            attack.Key,
                            attack.Value[0].ToString("N"),
                            "N/A"
                        });
                }
            }

            _statsReporting.ReportTable(table, "Attacks by NPC", Fight, State, length);
        }

        public override void StartFight()
        {
            
        }
    }
}
