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
        public CalculatorFactory(List<ICalculator> calculators)
        {
            CalculatorFlatList = calculators;
            foreach (var calc in calculators)
                foreach(var evnt in calc.ApplicableEvents)
                {
                    if (Calculators.TryGetValue(evnt, out var list))
                        list.Add(calc);
                    else
                        Calculators.Add(evnt, new List<ICalculator>() { calc });
                }
        }

        public void CalculateEvent(CombatEventBase combatEvent)
        {
            if (Calculators.TryGetValue(combatEvent.EventName, out var calcList))
                foreach (var calc in calcList)
                    calc.CalculateEvent(combatEvent);
        }

        public void FinalizeCalculations()
        {
            foreach (var calc in CalculatorFlatList)
                calc.FinalizeCalculations();
        }


        public void StartFight(MonitoredFight fight)
        {
            foreach (var calc in CalculatorFlatList)
                calc.StartFight(fight);
        }

        public void FinalizeFight(MonitoredFight fight)
        {
            foreach (var calc in CalculatorFlatList)
                calc.FinalizeFight(fight);
        }
    }
}
