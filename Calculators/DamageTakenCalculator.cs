using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Calculators
{
    public class DamageTakenCalculator : ICalculator
    {
        Dictionary<string, Dictionary<string, long>> _damageTakenByEntityFromEntity = new Dictionary<string, Dictionary<string, long>>();

        public List<string> ApplicableEvents { get; set; } = new List<string>()
        {
            LogEvents.SPELL_CAST_SUCCESS,
            LogEvents.RANGE_DAMAGE,
            LogEvents.SWING_DAMAGE,
            LogEvents.SPELL_PERIODIC_DAMAGE
        };

        public void CalculateEvent(CombatEventBase combatEvent)
        {
            var damage = (IDamage)combatEvent;
        }

        public void FinalizeCalculations()
        {
            
        }
    }
}
