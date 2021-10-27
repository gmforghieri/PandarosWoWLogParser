using PandarosWoWLogParser.Calculators;
using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;
using System.Collections.Generic;

namespace PandarosWoWLogParser
{
    public interface ICombatState
    {
        CalculatorFactory CalculatorFactory { get; set; }
        MonitoredFight CurrentFight { get; set; }
        Dictionary<string, string> EntitytoOwnerMap { get; set; }
        bool InFight { get; set; }
        Dictionary<string, List<string>> OwnerToEntityMap { get; set; }
        Dictionary<string, Dictionary<string, string>> PlayerBuffs { get; set; }
        Dictionary<string, Dictionary<string, string>> PlayerDebuffs { get; set; }

        void ParseComplete();
        void ProcessCombatEvent(ICombatEvent combatEvent, string evtStr);
        bool TryGetOwnerName(ICombatEvent combatEvent, out string owner);
    }
}