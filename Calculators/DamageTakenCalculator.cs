using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Calculators
{
    public class DamageTakenCalculator : BaseCalculator
    {
        Dictionary<string, Dictionary<string, Dictionary<string, List<long>>>> _damageTakenByEntityFromEntity = new Dictionary<string, Dictionary<string, Dictionary<string, List<long>>>>();

        public DamageTakenCalculator(IPandaLogger logger, IStatsReporter reporter, ICombatState state, MonitoredFight fight) : base(logger, reporter, state, fight)
        {
            ApplicableEvents = new List<string>()
                {
                    LogEvents.SPELL_DAMAGE,
                    LogEvents.RANGE_DAMAGE,
                    LogEvents.SWING_DAMAGE,
                    LogEvents.SPELL_PERIODIC_DAMAGE,
                    LogEvents.DAMAGE_SHIELD
                };
        }


        public override void CalculateEvent(ICombatEvent combatEvent)
        {
            var damage = (IDamage)combatEvent;

            if (combatEvent.SourceFlags.GetFlagType == UnitFlags.FlagType.Npc && combatEvent.DestFlags.GetFlagType == UnitFlags.FlagType.Player)
            {

                if (combatEvent.EventName == LogEvents.SPELL_DAMAGE ||
                    combatEvent.EventName == LogEvents.SPELL_DAMAGE ||
                    combatEvent.EventName == LogEvents.SPELL_DAMAGE ||
                    combatEvent.EventName == LogEvents.SPELL_DAMAGE)
                {
                    var spell = (ISpell)combatEvent;
                    _damageTakenByEntityFromEntity.AddValue(combatEvent.DestName, combatEvent.SourceName, spell.SpellName, 0, 1);
                    _damageTakenByEntityFromEntity.AddValue(combatEvent.DestName, combatEvent.SourceName, spell.SpellName, 1, damage.Damage);
                }
                else
                {
                    _damageTakenByEntityFromEntity.AddValue(combatEvent.DestName, combatEvent.SourceName, "Swing", 0, 1);
                    _damageTakenByEntityFromEntity.AddValue(combatEvent.DestName, combatEvent.SourceName, "Swing", 1, damage.Damage);
                }
            }
        }

        public override void FinalizeFight()
        {
            List<List<string>> table = new List<List<string>>();
            table.Add(new List<string>()
            {
                "Player",
                "NPC",
                "Attack",
                "Count",
                "Damage"
            });

            var length = new List<int>()
            {
                20,
                35,
                30,
                13,
                13
            };

            foreach (var pc in _damageTakenByEntityFromEntity)
            {
                table.Add(new List<string>()
                {
                    pc.Key,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty
                });
                foreach (var npc in pc.Value)
                {
                    table.Add(new List<string>()
                    {
                        string.Empty,
                        npc.Key,
                        string.Empty,
                        string.Empty,
                        string.Empty
                    });

                    foreach (var attack in npc.Value)
                    {
                        table.Add(new List<string>()
                        {
                            string.Empty,
                            string.Empty,
                            attack.Key,
                            attack.Value[0].ToString("N"),
                            attack.Value[1].ToString("N")
                        });
                    }
                }
            }

            _statsReporting.ReportTable(table, "Damage Taken", Fight, State, length);
        }

        public override void StartFight()
        {
            
        }
    }
}
