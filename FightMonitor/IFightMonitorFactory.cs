using PandarosWoWLogParser.Calculators;
using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;

namespace PandarosWoWLogParser.FightMonitor
{
    public interface IFightMonitorFactory
    {
        Dictionary<string, Tuple<string, MonitoredZone>> MonitoredBosses { get; set; }
        List<MonitoredZone> MonitoredZones { get; set; }
        bool IsMonitoredFight(ICombatEvent evnt, ICombatState state);
    }
}