using PandarosWoWLogParser.Calculators;
using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;
using System.Collections.Generic;

namespace PandarosWoWLogParser
{
    public abstract class CombatStateBase : ICombatState
    {
        internal IFightMonitorFactory _fightMonitorFactory;
        internal IPandaLogger _logger;
        internal Dictionary<string, int> unknown = new Dictionary<string, int>();
        internal Dictionary<string, int> eventCount = new Dictionary<string, int>();
        internal bool _prevFightState = false;

        public Dictionary<string, List<string>> OwnerToEntityMap { get; set; } = new Dictionary<string, List<string>>();

        public Dictionary<string, string> EntitytoOwnerMap { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, Dictionary<string, string>> PlayerBuffs { get; set; } = new Dictionary<string, Dictionary<string, string>>();
        public Dictionary<string, Dictionary<string, string>> PlayerDebuffs { get; set; } = new Dictionary<string, Dictionary<string, string>>();

        public bool InFight { get; set; }
        public MonitoredFight CurrentFight { get; set; }
        public CalculatorFactory CalculatorFactory { get; set; }

        public CombatStateBase(IFightMonitorFactory fightMonitorFactory, IPandaLogger logger)
        {
            _fightMonitorFactory = fightMonitorFactory;
            _logger = logger;
        }

        public virtual void ParseComplete()
        {
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

        public virtual void ProcessCombatEvent(ICombatEvent combatEvent, string evtStr)
        {
            if (combatEvent == null)
            {
                if (!string.IsNullOrEmpty(evtStr))
                    unknown.AddValue(evtStr, 1);
            }
            else
            {
                eventCount.AddValue(combatEvent.EventName, 1);
            }
        }

        public virtual bool TryGetSourceOwnerName(ICombatEvent combatEvent, out string owner)
        {
            owner = null;
            return combatEvent.SourceFlags.GetController == UnitFlags.Controller.Player &&
                EntitytoOwnerMap.TryGetValue(combatEvent.SourceGuid, out owner);
        }

        public virtual bool TryGetDestOwnerName(ICombatEvent combatEvent, out string owner)
        {
            owner = null;
            return combatEvent.DestFlags.GetController == UnitFlags.Controller.Player &&
                EntitytoOwnerMap.TryGetValue(combatEvent.DestGuid, out owner);
        }

        internal virtual void ProcessCombatEventInternal(ICombatEvent combatEvent)
        {
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
    }
}