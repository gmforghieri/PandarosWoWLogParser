using PandarosWoWLogParser.FightMonitor;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser
{
    public interface IStatsReporter
    {
        public void ReportPerSecondNumbers<T>(Dictionary<T, long> stats, string name, MonitoredFight fight, CombatState state);
        public void Report<T>(Dictionary<T, long> stats, string name, MonitoredFight fight, CombatState state);
        public void Report<T, G>(Dictionary<T, Dictionary<G, long>> stats, string name, MonitoredFight fight, CombatState state);
        public void Report<T, G, B>(Dictionary<T, Dictionary<G, Dictionary<B, long>>> stats, string name, MonitoredFight fight, CombatState state);
        public void ReportTable(List<List<string>> table, string name, MonitoredFight fight, CombatState state, List<int> length = default(List<int>));
    }
}
