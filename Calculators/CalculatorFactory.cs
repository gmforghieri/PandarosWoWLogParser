using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Calculators
{
    public class CalculatorFactory : ICalculatorFactory
    {
        public Dictionary<string, List<ICalculator>> Calculators { get; set; } = new Dictionary<string, List<ICalculator>>();

        public CalculatorFactory(List<ICalculator> calculators)
        {
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
            foreach (var calcKvp in Calculators)
                foreach (var calc in calcKvp.Value)
                    calc.FinalizeCalculations();
        }
    }
}
