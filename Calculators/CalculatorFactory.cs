using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Calculators
{
    public class CalculatorFactory : ICalculatorFactory
    {
        public Dictionary<string, List<ICalculator>> Calculators { get; set; } = new Dictionary<string, List<ICalculator>>();

        public CalculatorFactory()
        {

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
