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
        IPandaLogger _logger;

        public TotalDamageCalculator(IPandaLogger logger)
        {
            _logger = logger;
        }

        public List<string> ApplicableEvents { get; set; } = new List<string>()
        {
            LogEvents.SPELL_DAMAGE,
            LogEvents.RANGE_DAMAGE,
            LogEvents.SWING_DAMAGE,
            LogEvents.SPELL_PERIODIC_DAMAGE
        };

        public void CalculateEvent(ICombatEvent combatEvent, CombatState state)
        {
            var damage = (IDamage)combatEvent;

            if (combatEvent.SourceFlags.GetFlagType() == UnitFlags.FlagType.Player)
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

            if (state.InFight && combatEvent.SourceFlags.GetFlagType() == UnitFlags.FlagType.Player)
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

        public void FinalizeCalculations(CombatState state)
        {
            int i = 0;
            _logger.Log("---------------------------------------------");
            _logger.Log("DPS Rankings");
            _logger.Log("---------------------------------------------");
            foreach (var person in _damageDoneByPlayersTotal.OrderBy(i => i.Value).Reverse())
            {
                i++;
                _logger.Log($"{i}. {person.Key}: {person.Value.ToString("N")}");
            }
            _logger.Log("---------------------------------------------");
        }

        public void FinalizeFight(MonitoredFight fight, CombatState state)
        {
            int i = 0;
            _logger.Log("---------------------------------------------");
            _logger.Log($"DPS Rankings: {fight.CurrentZone.ZoneName} - {fight.BossName}");
            _logger.Log("---------------------------------------------");
            foreach (var person in _damageDoneByPlayersFight.OrderBy(i => i.Value).Reverse())
            {
                i++;
                _logger.Log($"{i}. {person.Key}: {person.Value.ToString("N")}");
            }
            _logger.Log("---------------------------------------------");
        }

        public void StartFight(MonitoredFight fight, CombatState state)
        {
            _damageDoneByPlayersFight.Clear();
        }
    }
}
