using PandarosWoWLogParser.FightMonitor;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser
{
    public interface IStatsReporter
    {
        public void ReportPerSecondNumbers(Dictionary<string, long> stats, string name, MonitoredFight fight, CombatState state);
        public void Report(Dictionary<string, long> stats, string name, MonitoredFight fight, CombatState state);
        public void Report(Dictionary<string, Dictionary<string, long>> stats, string name, MonitoredFight fight, CombatState state);
    }
}
