using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;

namespace PandarosWoWLogParser.FightMonitor
{
    public interface IFightMonitorFactory
    {
        MonitoredFight CurrentFight { get; set; }
        bool IsInFight { get; set; }
        Dictionary<string, Tuple<string, MonitoredZone>> MonitoredBosses { get; set; }
        List<MonitoredZone> MonitoredZones { get; set; }

        MonitoredFight GetFight();
        bool IsMonitoredFight(ICombatEvent evnt, CombatState state);
    }
}