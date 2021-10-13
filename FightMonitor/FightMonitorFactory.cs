using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PandarosWoWLogParser.FightMonitor
{
    public class FightMonitorFactory : IFightMonitorFactory
    {
        public List<MonitoredZone> MonitoredZones { get; set; }

        public Dictionary<string, MonitoredZone> MonitoredBosses { get; set; } = new Dictionary<string, MonitoredZone>();

        public bool IsInFight { get; set; }

        public MonitoredFight CurrentFight { get; set; }

        public FightMonitorFactory(List<MonitoredZone> monitoredZones)
        {
            MonitoredZones = monitoredZones;

            foreach (var zone in monitoredZones)
                foreach (var boss in zone.MonitoredFights)
                    MonitoredBosses[boss] = zone;
        }

        public bool IsMonitoredFight(ICombatEvent evnt)
        {
            if (!IsInFight)
            {
                IsInFight = MonitoredBosses.ContainsKey(evnt.DestName) || MonitoredBosses.ContainsKey(evnt.SourceName);

                if (IsInFight)
                    CurrentFight = new MonitoredFight()
                    {
                        CurrentZone = MonitoredBosses[evnt.DestName],
                        FightStart = evnt.Timestamp,
                        MonsterName = evnt.DestName
                    };
            }

            if (IsInFight)
            {
                IsInFight = CurrentFight.AddEvent(evnt);
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
