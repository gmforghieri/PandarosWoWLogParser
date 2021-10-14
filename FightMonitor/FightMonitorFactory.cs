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

        public bool IsInFight { get; set; }

        public MonitoredFight CurrentFight { get; set; }

        public FightMonitorFactory(List<MonitoredZone> monitoredZones)
        {
            MonitoredZones = monitoredZones;

            foreach (var zone in monitoredZones)
                foreach (var bossList in zone.MonitoredFights)
                    foreach (var boss in bossList.Value)
                        MonitoredBosses[boss] = Tuple.Create(bossList.Key, zone);
        }

        public bool IsMonitoredFight(ICombatEvent evnt, CombatState state)
        {
            if (!IsInFight && CombatEventsTriggerInFight.Contains(evnt.EventName))
            {
                if (MonitoredBosses.ContainsKey(evnt.DestName))
                {
                    IsInFight = true;
                    CurrentFight = new MonitoredFight()
                    {
                        CurrentZone = MonitoredBosses[evnt.DestName].Item2,
                        BossName = MonitoredBosses[evnt.DestName].Item1,
                        FightStart = evnt.Timestamp,
                        MonsterID = new Dictionary<string, bool>() { { evnt.DestName, false } }
                    };
                }
                else if (MonitoredBosses.ContainsKey(evnt.SourceName))
                {
                    IsInFight = true;
                    CurrentFight = new MonitoredFight()
                    {
                        CurrentZone = MonitoredBosses[evnt.SourceName].Item2,
                        BossName = MonitoredBosses[evnt.SourceName].Item1,
                        FightStart = evnt.Timestamp,
                        MonsterID = new Dictionary<string, bool>() { { evnt.SourceName, false } }
                    };
                }
            }

            if (IsInFight)
            {
                IsInFight = CurrentFight.AddEvent(evnt, state);
            }

            return IsInFight;
        }

        public MonitoredFight GetFight()
        {
            var retval = CurrentFight;
            CurrentFight = null;
            return retval;
        }
    }
}
