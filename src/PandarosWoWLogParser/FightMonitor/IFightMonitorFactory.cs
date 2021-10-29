using PandarosWoWLogParser.Calculators;
using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;

namespace PandarosWoWLogParser.FightMonitor
{
    public interface IFightMonitorFactory
    {
        bool IsMonitoredFight(ICombatEvent evnt, ICombatState state);
    }
}