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

        Dictionary<string, long> _healingDoneByPlayersFight = new Dictionary<string, long>();
        Dictionary<string, long> _overHealingDoneByPlayersFight = new Dictionary<string, long>();
        Dictionary<string, long> _normalDoneByPlayersFight = new Dictionary<string, long>();

        public List<string> ApplicableEvents { get; set; } = new List<string>()
        {
            LogEvents.SPELL_HEAL,
            LogEvents.SPELL_PERIODIC_HEAL
        };

        IPandaLogger _logger;

        public HealingDoneCalculator(IPandaLogger logger)
        {
            _logger = logger;
        }

        public void CalculateEvent(ICombatEvent combatEvent, CombatState state)
        {
            var damage = (ISpellHeal)combatEvent;

            if (combatEvent.SourceFlags.GetFlagType() == UnitFlags.FlagType.Player && combatEvent.SourceFlags.GetFlagType() == UnitFlags.FlagType.Player)
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

                if (state.InFight)
                {
                    if (_healingDoneByPlayersFight.TryGetValue(combatEvent.SourceName, out long healFight))
                    {
                        _healingDoneByPlayersFight[combatEvent.SourceName] = healFight + damage.HealAmount + damage.Overhealing;
                    }
                    else
                    {
                        _healingDoneByPlayersFight[combatEvent.SourceName] = damage.HealAmount + damage.Overhealing;
                    }

                    if (_overHealingDoneByPlayersFight.TryGetValue(combatEvent.SourceName, out long overFight))
                    {
                        _overHealingDoneByPlayersFight[combatEvent.SourceName] = overFight + damage.Overhealing;
                    }
                    else
                    {
                        _overHealingDoneByPlayersFight[combatEvent.SourceName] = damage.Overhealing;
                    }

                    if (_normalDoneByPlayersFight.TryGetValue(combatEvent.SourceName, out long goodHealFight))
                    {
                        _normalDoneByPlayersFight[combatEvent.SourceName] = goodHealFight + damage.HealAmount;
                    }
                    else
                    {
                        _normalDoneByPlayersFight[combatEvent.SourceName] = damage.HealAmount;
                    }
                }
            }
        }

        public void FinalizeCalculations(CombatState state)
        {
            int i = 0;
            _logger.Log("---------------------------------------------");
            _logger.Log("Healing Rankings Total (Overheal_Life) [Healed])");
            _logger.Log("---------------------------------------------");
            foreach (var person in _healingDoneByPlayersTotal.OrderBy(i => i.Value).Reverse())
            {
                i++;
                _overHealingDoneByPlayersTotal.TryGetValue(person.Key, out var overheal);
                _normalDoneByPlayersTotal.TryGetValue(person.Key, out var goodHeal);
                _logger.Log($"{i}. {person.Key}: {person.Value.ToString("N")} ({overheal.ToString("N")}) [{goodHeal.ToString("N")}]");
            }
            i = 0;
            _logger.Log("---------------------------------------------");
            _logger.Log("---------------------------------------------");
            _logger.Log("Overhealed Rankings Total");
            _logger.Log("---------------------------------------------");
            foreach (var person in _overHealingDoneByPlayersTotal.OrderBy(i => i.Value).Reverse())
            {
                i++;
                _logger.Log($"{i}. {person.Key}: {person.Value.ToString("N")}");
            }
            i = 0;
            _logger.Log("---------------------------------------------");
            _logger.Log("---------------------------------------------");
            _logger.Log("Healed Rankings Total");
            _logger.Log("---------------------------------------------");
            foreach (var person in _normalDoneByPlayersTotal.OrderBy(i => i.Value).Reverse())
            {
                i++;
                _logger.Log($"{i}. {person.Key}: {person.Value.ToString("N")}");
            }
            _logger.Log("---------------------------------------------");
        }

        public void FinalizeFight(MonitoredFight fight, CombatState state)
        {
            int i = 0;
            var ts = fight.FightEnd.Subtract(fight.FightStart);
            _logger.Log("---------------------------------------------");
            _logger.Log($"Healing Rankings {fight.CurrentZone.ZoneName}: {fight.BossName} (Overheal_Life) [Healed])");
            _logger.Log("---------------------------------------------");
            foreach (var person in _healingDoneByPlayersFight.OrderBy(i => i.Value).Reverse())
            {
                i++;
                _overHealingDoneByPlayersFight.TryGetValue(person.Key, out var overheal);
                _normalDoneByPlayersFight.TryGetValue(person.Key, out var goodHeal);
                _logger.Log($"{i}. {person.Key}: {person.Value.ToString("N")} ({overheal.ToString("N")}) [{goodHeal.ToString("N")}]");
            }

            _logger.Log("---------------------------------------------");
            _logger.Log($"HPS Rankings {fight.CurrentZone.ZoneName}: {fight.BossName}");
            _logger.Log("---------------------------------------------");
            foreach (var person in _healingDoneByPlayersFight.OrderBy(i => i.Value).Reverse())
            {
                i++;
                _logger.Log($"{i}. {person.Key}: {(person.Value / ts.TotalSeconds).ToString("N")}");
            }
            i = 0;
            _logger.Log("---------------------------------------------");
            _logger.Log("---------------------------------------------");
            _logger.Log($"Overhealed Rankings {fight.CurrentZone.ZoneName}: {fight.BossName}");
            _logger.Log("---------------------------------------------");
            foreach (var person in _overHealingDoneByPlayersFight.OrderBy(i => i.Value).Reverse())
            {
                i++;
                _logger.Log($"{i}. {person.Key}: {person.Value.ToString("N")}");
            }
            i = 0;
            _logger.Log("---------------------------------------------");
            _logger.Log("---------------------------------------------");
            _logger.Log($"Healed Rankings {fight.CurrentZone.ZoneName}: {fight.BossName}");
            _logger.Log("---------------------------------------------");
            foreach (var person in _normalDoneByPlayersFight.OrderBy(i => i.Value).Reverse())
            {
                i++;
                _logger.Log($"{i}. {person.Key}: {person.Value.ToString("N")}");
            }
            _logger.Log("---------------------------------------------");
        }

        public void StartFight(MonitoredFight fight, CombatState state)
        {
            _normalDoneByPlayersTotal.Clear();
            _overHealingDoneByPlayersTotal.Clear();
            _healingDoneByPlayersFight.Clear();
        }
    }
}
