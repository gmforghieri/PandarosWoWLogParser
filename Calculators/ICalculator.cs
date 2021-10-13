using System;
using System.Collections.Generic;
using System.Text;
using PandarosWoWLogParser.Models;

namespace PandarosWoWLogParser.Calculators
{
    public interface ICalculator
    {
        public List<string> ApplicableEvents { get; set; }
        public void CalculateEvent(CombatEventBase combatEvent);
        public void FinalizeCalculations();
    }
}
