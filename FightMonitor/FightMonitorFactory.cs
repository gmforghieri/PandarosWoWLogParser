using PandarosWoWLogParser.Calculators;
using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PandarosWoWLogParser.FightMonitor
{
    public class FightMonitorFactory : IFightMonitorFactory
    {
        public static List<string> CombatEventsTriggerInFight { get; set; } = new List<string>()
        {
            LogEvents.SPELL_DAMAGE,
            LogEvents.SPELL_PERIODIC_DAMAGE,
            LogEvents.RANGE_MISSED,
            LogEvents.RANGE_DAMAGE,
            LogEvents.SPELL_MISSED,
            LogEvents.SWING_DAMAGE,
            LogEvents.SWING_MISSED
        };

        public List<MonitoredZone> MonitoredZones { get; set; }

        public Dictionary<string, Tuple<string, MonitoredZone>> MonitoredBosses { get; set; } = new Dictionary<string, Tuple<string, MonitoredZone>>();

        IPandaLogger _logger;
        IStatsReporter _reporter;

        public FightMonitorFactory(List<MonitoredZone> monitoredZones, IPandaLogger logger, IStatsReporter reporter)
        {
            MonitoredZones = monitoredZones;
            _reporter = reporter;
            _logger = logger;

            foreach (var zone in monitoredZones)
                foreach (var bossList in zone.MonitoredFights)
                    foreach (var boss in bossList.Value)
                        MonitoredBosses[boss] = Tuple.Create(bossList.Key, zone);
        }

        public bool IsMonitoredFight(ICombatEvent evnt, CombatState state)
        {
            if (!state.InFight && CombatEventsTriggerInFight.Contains(evnt.EventName))
            {
                if (MonitoredBosses.ContainsKey(evnt.DestName))
                {
                    state.InFight = true;
                    state.CurrentFight = new MonitoredFight()
                    {
                        CurrentZone = MonitoredBosses[evnt.DestName].Item2,
                        BossName = MonitoredBosses[evnt.DestName].Item1,
                        FightStart = evnt.Timestamp,
                        MonsterID = new Dictionary<string, bool>() { { evnt.DestName, false } }
                    };
                    state.CalculatorFactory = new CalculatorFactory(_logger, _reporter, state);
                }
                else if (MonitoredBosses.ContainsKey(evnt.SourceName))
                {
                    state.InFight = true;
                    state.CurrentFight = new MonitoredFight()
                    {
                        CurrentZone = MonitoredBosses[evnt.SourceName].Item2,
                        BossName = MonitoredBosses[evnt.SourceName].Item1,
                        FightStart = evnt.Timestamp,
                        MonsterID = new Dictionary<string, bool>() { { evnt.SourceName, false } }
                    };
                    state.CalculatorFactory = new CalculatorFactory(_logger, _reporter, state);
                }
            }

            if (state.InFight)
            {
                state.InFight = state.CurrentFight.AddEvent(evnt, state);
            }

            return state.InFight;
        }
    }
}
