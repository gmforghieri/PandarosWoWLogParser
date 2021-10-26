using CombatLogParser;
using PandarosWoWLogParser.Calculators;
using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;
using PandarosWoWLogParser.Parsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser
{
    public class CombatState
    {
        IFightMonitorFactory _fightMonitorFactory;
        IPandaLogger _logger;
        IStatsReporter _reporter;
        Dictionary<string, int> unknown = new Dictionary<string, int>();
        Dictionary<string, int> eventCount = new Dictionary<string, int>();
        bool _prevFightState = false;
        ICalculatorFactory _allFightsCalculatorFactory;

        public CombatState(IFightMonitorFactory fightMonitorFactory, IPandaLogger logger, IStatsReporter reporter)
        {
            _fightMonitorFactory = fightMonitorFactory;
            _logger = logger;
            _reporter = reporter;
            MonitoredFight allFights = new MonitoredFight()
            {
                CurrentZone = new MonitoredZone()
                {
                    ZoneName = "All",
                    MonitoredFights = new Dictionary<string, List<string>>()
                },
                BossName = "All Fights in Log"
            };

            AllFights = allFights;

            _allFightsCalculatorFactory = new CalculatorFactory(_logger, _reporter, this, allFights);
        }

        public Dictionary<string, string> EntityIdToNameMap { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, List<string>> OwnerToEntityMap { get; set; } = new Dictionary<string, List<string>>();

        public Dictionary<string, string> EntitytoOwnerMap { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, Dictionary<string, string>> PlayerBuffs { get; set; } = new Dictionary<string, Dictionary<string, string>>();
        public Dictionary<string, Dictionary<string, string>> PlayerDebuffs { get; set; } = new Dictionary<string, Dictionary<string, string>>();

        public bool InFight { get; set; }
        public MonitoredFight CurrentFight { get; set; }
        public MonitoredFight AllFights { get; set; }
        public CalculatorFactory CalculatorFactory { get; set; }

        public void ProcessCombatEvent(ICombatEvent combatEvent, string evtStr)
        {
            if (combatEvent == null)
            {
                if (!string.IsNullOrEmpty(evtStr))
                    unknown.AddValue(evtStr, 1);
            }
            else
            {
                eventCount.AddValue(combatEvent.EventName, 1);

                ProcessCombatEventInternal(combatEvent);
                _allFightsCalculatorFactory.CalculateEvent(combatEvent);

                if (_fightMonitorFactory.IsMonitoredFight(combatEvent, this))
                    _prevFightState = true;
                else if (_prevFightState)
                {
                    CalculatorFactory.StartFight();

                    foreach (var fightEvent in CurrentFight.MonitoredFightEvents)
                    {
                        CalculatorFactory.CalculateEvent(fightEvent);
                    }

                    CalculatorFactory.FinalizeFight();

                    _prevFightState = false;
                    CleanUpFight();
                }

            }
        }
        private void ProcessCombatEventInternal(ICombatEvent combatEvent)
        {
            if (combatEvent.DestName != "nil" &&
                combatEvent.DestFlags.GetFlagType != UnitFlags.FlagType.Player)
            {
                EntityIdToNameMap[combatEvent.DestGuid] = combatEvent.DestName;
            }

            switch (combatEvent.EventName)
            {
                case LogEvents.SPELL_SUMMON:
                    if (combatEvent.SourceFlags.GetFlagType == UnitFlags.FlagType.Player)
                    {
                        if (!OwnerToEntityMap.TryGetValue(combatEvent.SourceName, out var list))
                        {
                            list = new List<string>();
                            OwnerToEntityMap[combatEvent.SourceName] = list;
                        }

                        if (!list.Contains(combatEvent.DestGuid))
                            list.Add(combatEvent.DestGuid);

                        if (!EntitytoOwnerMap.ContainsKey(combatEvent.DestGuid))
                            EntitytoOwnerMap.Add(combatEvent.DestGuid, combatEvent.SourceName);
                    }
                    break;

                case LogEvents.UNIT_DIED:
                    if (EntitytoOwnerMap.TryGetValue(combatEvent.DestGuid, out var ownerId))
                        EntitytoOwnerMap.Remove(combatEvent.DestGuid);

                    if (OwnerToEntityMap.TryGetValue(combatEvent.SourceName, out var entities))
                        entities.Remove(combatEvent.DestGuid);

                    EntitytoOwnerMap.Remove(combatEvent.DestGuid);
                    EntityIdToNameMap.Remove(combatEvent.DestGuid);
                    break;

                case LogEvents.SPELL_AURA_APPLIED:
                case LogEvents.SPELL_AURA_APPLIED_DOSE:
                case LogEvents.SPELL_AURA_REFRESH:
                    var spell = (ISpell)combatEvent;
                    var aura = (ISpellAura)combatEvent;

                    if (aura.AuraType == BuffType.Buff)
                        PlayerBuffs.AddValue(combatEvent.DestName, spell.SpellName, combatEvent.SourceName);
                    else
                        PlayerDebuffs.AddValue(combatEvent.DestName, spell.SpellName, combatEvent.SourceName);
                    break;

                case LogEvents.SPELL_AURA_BROKEN:
                case LogEvents.SPELL_AURA_REMOVED_DOSE:
                case LogEvents.SPELL_AURA_BROKEN_SPELL:
                    var removedSpell = (ISpell)combatEvent;

                    PlayerBuffs.RemoveValue(combatEvent.DestName, removedSpell.SpellName);
                    PlayerDebuffs.RemoveValue(combatEvent.DestName, removedSpell.SpellName);
                    break;
            }
            
        }

        public string GetEntityPrintName(string id)
        {
            if (EntityIdToNameMap.TryGetValue(id, out var name))
                return name;
            else
                return "Unknown";
        }

        public bool TryGetOwnerName(ICombatEvent combatEvent, out string owner)
        {
            owner = null;
            return combatEvent.SourceFlags.GetController == UnitFlags.Controller.Player &&
                EntitytoOwnerMap.TryGetValue(combatEvent.SourceGuid, out owner);
        }

        public void CleanUpFight()
        {
            CalculatorFactory = null;
            CurrentFight = null;
        }

        public void ParseComplete()
        {
            _allFightsCalculatorFactory.FinalizeFight();
            _logger.Log($"``````````````````````````````````````````````````````````````");
            _logger.Log($"Number of unknown events: {unknown.Count}");
            _logger.Log($"--------------------------------------------------------------");
            foreach (var ev in unknown)
                _logger.Log($"{ev.Key}: {ev.Value}");
            _logger.Log($"``````````````````````````````````````````````````````````````");

            _logger.Log($"Number of known events: {eventCount.Count}");
            _logger.Log($"--------------------------------------------------------------");
            foreach (var ev in eventCount)
                _logger.Log($"{ev.Key}: {ev.Value}");
            _logger.Log($"``````````````````````````````````````````````````````````````");
        }
    }
}
