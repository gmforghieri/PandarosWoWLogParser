using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Calculators
{
    public class CalculatorFactory : ICalculatorFactory
    {
        public Dictionary<string, List<ICalculator>> Calculators { get; set; } = new Dictionary<string, List<ICalculator>>();
        public List<ICalculator> CalculatorFlatList { get; set; }
        Dictionary<string, int> _eventCount = new Dictionary<string, int>();
        IPandaLogger _logger;

        public CalculatorFactory(List<ICalculator> calculators, IPandaLogger logger)
        {
            CalculatorFlatList = calculators;
            _logger = logger;

            foreach (var calc in calculators)
                foreach(var evnt in calc.ApplicableEvents)
                {
                    if (Calculators.TryGetValue(evnt, out var list))
                        list.Add(calc);
                    else
                        Calculators.Add(evnt, new List<ICalculator>() { calc });
                }
        }

        public void CalculateEvent(ICombatEvent combatEvent, CombatState state)
        {
            if (state.InFight)
            {
                if (!_eventCount.TryGetValue(combatEvent.EventName, out int val))
                    _eventCount[combatEvent.EventName] = 1;
                else
                    _eventCount[combatEvent.EventName] = val + 1;
            }

            if (Calculators.TryGetValue(combatEvent.EventName, out var calcList))
                foreach (var calc in calcList)
                    calc.CalculateEvent(combatEvent, state);
        }

        public void FinalizeCalculations(CombatState state)
        {
            foreach (var calc in CalculatorFlatList)
                calc.FinalizeCalculations(state);
        }


        public void StartFight(MonitoredFight fight, CombatState state)
        {
            state.InFight = true;
            _eventCount.Clear();
            _logger.Log("---------------------------------------------");
            _logger.Log($"{fight.FightStart.ToLocalTime()} Fight Start: {fight.CurrentZone.ZoneName} - {fight.BossName}");
            _logger.Log("---------------------------------------------");
            foreach (var calc in CalculatorFlatList)
                calc.StartFight(fight, state);
        }

        public void FinalizeFight(MonitoredFight fight, CombatState state)
        {
            state.InFight = false;
            foreach (var calc in CalculatorFlatList)
                calc.FinalizeFight(fight, state);


            _logger.Log("---------------------------------------------");
            _logger.Log($"{fight.FightEnd.ToLocalTime()} Fight End: {fight.CurrentZone.ZoneName} - {fight.BossName} ({fight.FightEnd.Subtract(fight.FightStart)})");
            foreach (var ev in _eventCount)
                _logger.Log($"{ev.Key}: {ev.Value}");
            _logger.Log("---------------------------------------------");
        }
    }
}
