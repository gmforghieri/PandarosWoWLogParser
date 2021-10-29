using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.FightMonitor
{
    public class MonitoredFight
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string BossName { get; set; }
        public Dictionary<string, bool> MonsterID { get; set; } = new Dictionary<string, bool>();
        public DateTime FightStart { get; set; }
        public DateTime FightEnd { get; set; }
        public List<ICombatEvent> MonitoredFightEvents { get; set; } = new List<ICombatEvent>();
        ICombatEvent _lastKnownLog;
        public List<Guid> ChildIds { get; set; } = new List<Guid>();
        public Guid ParentId { get; set; }

        public bool AddEvent(ICombatEvent combatEvent, ICombatState state)
        {
            MonitoredFightEvents.Add(combatEvent);

            if (FightMonitorFactory.CombatEventsTriggerInFight.Contains(combatEvent.EventName))
            {
                if (combatEvent.DestFlags.IsNPC && !MonsterID.ContainsKey(combatEvent.DestGuid))
                {
                    MonsterID.Add(combatEvent.DestGuid, false);

                    if (!BossName.Contains(combatEvent.DestName))
                        BossName += ", " + combatEvent.DestName;

                } else if (combatEvent.SourceFlags.IsNPC && !MonsterID.ContainsKey(combatEvent.SourceGuid))
                {
                    MonsterID.Add(combatEvent.SourceGuid, false);

                    if (!BossName.Contains(combatEvent.SourceName))
                        BossName += ", " + combatEvent.SourceName;
                }
            }

            bool combatOver = false;

            // all monsters dead
            if (combatEvent.EventName == LogEvents.UNIT_DIED && MonsterID.ContainsKey(combatEvent.DestGuid))
            {
                MonsterID[combatEvent.DestGuid] = true;
                bool allDead = true;

                foreach (var val in MonsterID.Values)
                    if (!val)
                    {
                        allDead = val;
                        break;
                    }

                combatOver = allDead;
            }

            if (_lastKnownLog != null && !MonsterID.ContainsKey(combatEvent.DestGuid) && !MonsterID.ContainsKey(combatEvent.SourceGuid))
            {
                var ts = combatEvent.Timestamp.Subtract(_lastKnownLog.Timestamp);

                if (ts.TotalSeconds > 60)
                    combatOver = true;
            }
            else if (MonsterID.ContainsKey(combatEvent.SourceGuid) || MonsterID.ContainsKey(combatEvent.DestGuid))
                _lastKnownLog = combatEvent;

            var NotMonitoredFightEvents = new List<ICombatEvent>();

            if (combatOver)
            {
                for (int i = MonitoredFightEvents.Count - 1; i != 0; i--)
                {
                    if (MonitoredFightEvents[i] == _lastKnownLog)
                    {
                        FightEnd = MonitoredFightEvents[i].Timestamp;
                        break;
                    }
                    else
                    {
                        NotMonitoredFightEvents.Add(MonitoredFightEvents[i]);
                    }
                }

                foreach (var unmonitor in NotMonitoredFightEvents)
                    MonitoredFightEvents.Remove(unmonitor);

                return false;
            }

            return true;
        }
    }
}
