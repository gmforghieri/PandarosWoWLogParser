using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.FightMonitor
{
    public class MonitoredFight
    {
        public MonitoredZone CurrentZone { get; set; }
        public string MonsterName { get; set; }
        public DateTime FightStart { get; set; }
        public DateTime FightEnd { get; set; }
        public List<ICombatEvent> MonitoredFightEvents { get; set; } = new List<ICombatEvent>();
        public List<ICombatEvent> NotMonitoredFightEvents { get; set; } = new List<ICombatEvent>();
        DateTime _lastKnownLog;

        public bool AddEvent(ICombatEvent combatEvent)
        {
            if (combatEvent.DestName != MonsterName && combatEvent.SourceName != MonsterName)
            {
                var ts = combatEvent.Timestamp.Subtract(_lastKnownLog);

                if (ts.TotalSeconds > 120)
                {
                    for(int i = MonitoredFightEvents.Count -1; i == 0; i--)
                    {
                        if (combatEvent.DestName == MonsterName)
                        {
                            break;
                        }
                        else
                        {
                            NotMonitoredFightEvents.Add(MonitoredFightEvents[i]);
                        }
                    }

                    foreach (var unmonitor in NotMonitoredFightEvents)
                        MonitoredFightEvents.Remove(unmonitor);

                    NotMonitoredFightEvents.Reverse();
                    NotMonitoredFightEvents.Add(combatEvent);
                    return false;
                }
            }
            else
                _lastKnownLog = combatEvent.Timestamp;

            MonitoredFightEvents.Add(combatEvent);
            return true;
        }
    }
}
