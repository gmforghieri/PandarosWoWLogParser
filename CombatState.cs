using CombatLogParser;
using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser
{
    public class CombatState
    {
        public Dictionary<string, string> EntityIdToNameMap { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, List<string>> OwnerToEntityMap { get; set; } = new Dictionary<string, List<string>>();

        public Dictionary<string, string> EntitytoOwnerMap { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, Dictionary<string, string>> PlayerBuffs { get; set; } = new Dictionary<string, Dictionary<string, string>>();
        public Dictionary<string, Dictionary<string, string>> PlayerDebuffs { get; set; } = new Dictionary<string, Dictionary<string, string>>();

        public bool InFight { get; set; }

        public void ProcessCombatEvent(ICombatEvent combatEvent)
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
    }
}
