using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PandarosWoWLogParser.Calculators
{
    public class MinGCDCalculator : BaseCalculator
    {
        Dictionary<string, DateTime> _spellsCast = new Dictionary<string, DateTime>();
        Dictionary<string, TimeSpan> _minTimes = new Dictionary<string, TimeSpan>();

        public MinGCDCalculator(IPandaLogger logger, IStatsReporter reporter, ICombatState state, MonitoredFight fight) : base(logger, reporter, state, fight)
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

            if (_spellsCast.TryGetValue(combatEvent.SourceName, out var lastCastTime))
            {
                var ts = combatEvent.Timestamp - lastCastTime;

                if (_minTimes.TryGetValue(combatEvent.SourceName, out var existingTs))
                {
                    if (ts < existingTs)
                        _minTimes[combatEvent.SourceName] = ts;
                }
                else
                    _minTimes.Add(combatEvent.SourceName, ts);
            }

            _spellsCast[combatEvent.SourceName] = combatEvent.Timestamp;
        }

        public override void FinalizeFight()
        {
            Dictionary<string, long> report = new Dictionary<string, long>();

            foreach (var ts in _minTimes)
                report[ts.Key] = Convert.ToInt64(ts.Value.TotalMilliseconds);

            _statsReporting.Report(report, "Min Time Between Casts in milliseconds", Fight, State);
        }

        public override void StartFight()
        {
            
        }
    }
}
