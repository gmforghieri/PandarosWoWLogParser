using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Calculators
{
    public class MissedCalculator : BaseCalculator
    {
        Dictionary<string, Dictionary<MissType, long>> _damageAvoidedByEntityfromType = new Dictionary<string, Dictionary<MissType, long>>();
        Dictionary<string, long> _attacks = new Dictionary<string, long>();
        Dictionary<string, Dictionary<string, long>> _missedAttacks = new Dictionary<string, Dictionary<string, long>>();

        public MissedCalculator(IPandaLogger logger, IStatsReporter reporter, CombatState state, MonitoredFight fight) : base(logger, reporter, state, fight)
        {
            ApplicableEvents = new List<string>()
                {
                    LogEvents.SWING_MISSED,
                    LogEvents.SWING_DAMAGE,
                    LogEvents.RANGE_DAMAGE,
                    LogEvents.SPELL_DAMAGE,
                    LogEvents.RANGE_MISSED,
                    LogEvents.SPELL_MISSED
                };
        }


        public override void CalculateEvent(ICombatEvent combatEvent)
        {
            if (combatEvent.DestFlags.GetFlagType == UnitFlags.FlagType.Npc)
            {
                _attacks.AddValue(combatEvent.SourceName, 1);

                switch (combatEvent.EventName)
                {
                    case LogEvents.SWING_MISSED:
                    case LogEvents.RANGE_MISSED:
                    case LogEvents.SPELL_MISSED:
                        var missed = (IMissed)combatEvent;

                        if (missed.MissType != MissType.ABSORB)
                        {
                            _damageAvoidedByEntityfromType.AddValue(combatEvent.SourceName, missed.MissType, 1);

                            if (combatEvent.EventName == LogEvents.SWING_MISSED)
                            {
                                _missedAttacks.AddValue(combatEvent.SourceName, "Swing", 1);
                            }
                            else
                            {
                                var spell = (ISpellBase)combatEvent;
                                _missedAttacks.AddValue(combatEvent.SourceName, spell.SpellName, 1);
                            }
                        }
                        break;
                }

            }
        }

        public override void FinalizeFight()
        {
            List<List<string>> table = new List<List<string>>();
            var enums = Enum.GetValues(typeof(MissType)).Cast<MissType>().ToList();
            enums.Remove(MissType.ABSORB);
            var all = enums.Select(e => e.ToString()).ToList();
            var total = _attacks.Sum(kvp => kvp.Value);
            all.Insert(0, $"Overall");
            all.Insert(0, $"attacksByPlayer");
            table.Add(all);
            Dictionary<string, long> totals = new Dictionary<string, long>();

            foreach (var baseKvp in _damageAvoidedByEntityfromType)
            {
                var subTotal = baseKvp.Value.Sum(kvp => kvp.Value);
                total += subTotal;
                totals[baseKvp.Key] = subTotal;
            }
            
            foreach (var baseKvp in totals.OrderBy(i => i.Value).Reverse())
            {
                var row = new List<string>();
                if (_attacks.TryGetValue(baseKvp.Key, out var attacksVsPlayer))
                {
                    row.Add($"{baseKvp.Key} ({attacksVsPlayer})");
                    row.Add($"{baseKvp.Value} ({(Math.Round((double)baseKvp.Value / attacksVsPlayer, 2) * 100).ToString().PadRight(3).Substring(0, 3) }%)");
                    foreach (var missType in enums)
                    {
                        if (_damageAvoidedByEntityfromType[baseKvp.Key].TryGetValue(missType, out var missCount))
                            row.Add($"{missCount} ({(Math.Round((double)missCount / attacksVsPlayer, 2) * 100).ToString().PadRight(3).Substring(0, 3)}%)");
                        else
                            row.Add("0");
                    }
                }
                else
                {
                    row.Add(baseKvp.Key);
                    row.Add("0");
                    foreach (var missType in enums)
                        row.Add("0");
                }

                table.Add(row);
            }

            _statsReporting.ReportTable(table, "Missed Attacks", Fight, State);
            _statsReporting.Report(_missedAttacks, "Missed Attack Breakdown", Fight, State);
        }

        public override void StartFight()
        {
            
        }
    }
}
