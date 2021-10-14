using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PandarosWoWLogParser.Calculators
{
    public class TotalDamageCalculator : ICalculator
    {
        Dictionary<string, long> _damageDoneByPlayersTotal = new Dictionary<string, long>();
        Dictionary<string, long> _damageDoneByPlayersFight = new Dictionary<string, long>();
        Dictionary<string, long> _damageDoneByNPCTotal = new Dictionary<string, long>();
        Dictionary<string, Dictionary<SpellSchool, long>> _damageDoneBySchoolTotal = new Dictionary<string, Dictionary<SpellSchool, long>>();
        bool _inFight = false;

        public List<string> ApplicableEvents { get; set; } = new List<string>()
        {
            LogEvents.SPELL_DAMAGE,
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

            if (_inFight && combatEvent.SourceFlags.GetController() == UnitFlags.Controller.Player)
            {
                if (_damageDoneByPlayersFight.TryGetValue(combatEvent.SourceName, out long dmgTotal))
                {
                    _damageDoneByPlayersFight[combatEvent.SourceName] = dmgTotal + damage.Damage;
                }
                else
                {
                    _damageDoneByPlayersFight[combatEvent.SourceName] = damage.Damage;
                }
            }
        }

        public void FinalizeCalculations()
        {
            int i = 0;
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("DPS Rankings");
            Console.WriteLine("---------------------------------------------");
            foreach (var person in _damageDoneByPlayersTotal.OrderBy(i => i.Value).Reverse())
            {
                i++;
                Console.WriteLine($"{i}. {person.Key}: {person.Value.ToString("N")}");
            }
            Console.WriteLine("---------------------------------------------");
        }

        public void FinalizeFight(MonitoredFight fight)
        {
            int i = 0;
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine($"DPS Rankings: {fight.CurrentZone} - {fight.MonsterName} ({fight.FightEnd.Subtract(fight.FightStart)})");
            Console.WriteLine("---------------------------------------------");
            foreach (var person in _damageDoneByPlayersFight.OrderBy(i => i.Value).Reverse())
            {
                i++;
                Console.WriteLine($"{i}. {person.Key}: {person.Value.ToString("N")}");
            }
            Console.WriteLine("---------------------------------------------");
            _inFight = false;
        }

        public void StartFight(MonitoredFight fight)
        {
            _damageDoneByPlayersFight.Clear();
            _inFight = true;
        }
    }
}
