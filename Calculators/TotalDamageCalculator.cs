using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Calculators
{
    public class TotalDamageCalculator : ICalculator
    {
        Dictionary<string, long> _damageDoneByPlayersTotal = new Dictionary<string, long>();
        Dictionary<string, long> _damageDoneByNPCTotal = new Dictionary<string, long>();
        Dictionary<string, Dictionary<SpellSchool, long>> _damageDoneBySchoolTotal = new Dictionary<string, Dictionary<SpellSchool, long>>();

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

            if (combatEvent.SourceFlags.GetController() == UnitFlags.Controller.Player)
            {
                if (_damageDoneByPlayersTotal.TryGetValue(combatEvent.SourceName, out long dmgTotal))
                {
                    _damageDoneByPlayersTotal[combatEvent.SourceName] = dmgTotal + damage.Damage;
                }
                else
                {
                    _damageDoneByPlayersTotal[combatEvent.SourceName] = damage.Damage;
                }
            }
        }

        public void FinalizeCalculations()
        {
            
        }
    }
}
