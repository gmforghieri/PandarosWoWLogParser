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

        public bool InFight { get; set; }

        public void ProcessCombatEvent(ICombatEvent combatEvent)
        {
            if (combatEvent.SourceName != "nil" && !EntityIdToNameMap.ContainsKey(combatEvent.SourceGuid))
                EntityIdToNameMap.Add(combatEvent.SourceGuid, combatEvent.SourceName);

            if (!EntityIdToNameMap.ContainsKey(combatEvent.DestGuid))
            {
                EntityIdToNameMap.Add(combatEvent.DestGuid, combatEvent.DestName);

                if (combatEvent.EventName == LogEvents.SPELL_SUMMON)
                {
                    if (!OwnerToEntityMap.TryGetValue(combatEvent.SourceGuid, out var list))
                    {
                        list = new List<string>();
                        OwnerToEntityMap[combatEvent.SourceGuid] = list;
                    }

                    if (!list.Contains(combatEvent.DestGuid))
                        list.Add(combatEvent.DestGuid);

                    if (!EntitytoOwnerMap.ContainsKey(combatEvent.DestGuid))
                        EntitytoOwnerMap.Add(combatEvent.DestGuid, combatEvent.SourceGuid);
                }
            }
            else
            {
                if (combatEvent.EventName == LogEvents.UNIT_DIED)
                {
                    if (EntitytoOwnerMap.TryGetValue(combatEvent.DestGuid, out var ownerId))
                        EntitytoOwnerMap.Remove(combatEvent.DestGuid);

                    if (!string.IsNullOrEmpty(ownerId) && OwnerToEntityMap.TryGetValue(ownerId, out var entities))
                        entities.Remove(combatEvent.DestGuid);

                    EntitytoOwnerMap.Remove(combatEvent.DestGuid);
                }
            }
        }

        public string GetEntityPrintName(string id)
        {
            if (EntityIdToNameMap.TryGetValue(id, out var name))
                return name;
            else
                return "Unknown";
        }
    }
}
