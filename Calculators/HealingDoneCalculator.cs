using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PandarosWoWLogParser.Calculators
{
    public class HealingDoneCalculator : ICalculator
    {
        Dictionary<string, long> _healingDoneByPlayersTotal = new Dictionary<string, long>();
        Dictionary<string, long> _overHealingDoneByPlayersTotal = new Dictionary<string, long>();
        Dictionary<string, long> _normalDoneByPlayersTotal = new Dictionary<string, long>();

        public List<string> ApplicableEvents { get; set; } = new List<string>()
        {
            LogEvents.SPELL_HEAL,
            LogEvents.SPELL_PERIODIC_HEAL
        };

        public void CalculateEvent(CombatEventBase combatEvent)
        {
            var damage = (ISpellHeal)combatEvent;

            if (combatEvent.SourceFlags.GetController() == UnitFlags.Controller.Player && combatEvent.SourceFlags.GetFlagType() == UnitFlags.FlagType.Player)
            {
                if (_healingDoneByPlayersTotal.TryGetValue(combatEvent.SourceName, out long healTotal))
                {
                    _healingDoneByPlayersTotal[combatEvent.SourceName] = healTotal + damage.HealAmount + damage.Overhealing;
                }
                else
                {
                    _healingDoneByPlayersTotal[combatEvent.SourceName] = damage.HealAmount + damage.Overhealing;
                }

                if (_overHealingDoneByPlayersTotal.TryGetValue(combatEvent.SourceName, out long overHeal))
                {
                    _overHealingDoneByPlayersTotal[combatEvent.SourceName] = overHeal + damage.Overhealing;
                }
                else
                {
                    _overHealingDoneByPlayersTotal[combatEvent.SourceName] = damage.Overhealing;
                }

                if (_normalDoneByPlayersTotal.TryGetValue(combatEvent.SourceName, out long goodHeal))
                {
                    _normalDoneByPlayersTotal[combatEvent.SourceName] = goodHeal + damage.HealAmount;
                }
                else
                {
                    _normalDoneByPlayersTotal[combatEvent.SourceName] = damage.HealAmount;
                }
            }
        }

        public void FinalizeCalculations()
        {
            int i = 0;
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("Healing Rankings Total (Overheal_Life) [Healed])");
            Console.WriteLine("---------------------------------------------");
            foreach (var person in _healingDoneByPlayersTotal.OrderBy(i => i.Value).Reverse())
            {
                i++;
                _overHealingDoneByPlayersTotal.TryGetValue(person.Key, out var overheal);
                _normalDoneByPlayersTotal.TryGetValue(person.Key, out var goodHeal);
                Console.WriteLine($"{i}. {person.Key}: {person.Value.ToString("N")} ({overheal.ToString("N")}) [{goodHeal.ToString("N")}]");
            }
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("Overhealed Rankings Total");
            Console.WriteLine("---------------------------------------------");
            foreach (var person in _overHealingDoneByPlayersTotal.OrderBy(i => i.Value).Reverse())
            {
                i++;
                Console.WriteLine($"{i}. {person.Key}: {person.Value.ToString("N")}");
            }
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("Healed Rankings Total");
            Console.WriteLine("---------------------------------------------");
            foreach (var person in _normalDoneByPlayersTotal.OrderBy(i => i.Value).Reverse())
            {
                i++;
                Console.WriteLine($"{i}. {person.Key}: {person.Value.ToString("N")}");
            }
            Console.WriteLine("---------------------------------------------");
        }

        public void FinalizeFight(MonitoredFight fight)
        {
            
        }

        public void StartFight(MonitoredFight fight)
        {
            
        }
    }
}
