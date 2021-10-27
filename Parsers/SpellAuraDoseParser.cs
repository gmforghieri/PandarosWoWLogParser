using PandarosWoWLogParser.Models;
using System;

namespace PandarosWoWLogParser.Parsers
{
    public class SpellAuraDoseParser : SpellParser, ICombatParser<SpellAuraDose>
    {
        public new SpellAuraDose Parse(DateTime timestamp, string eventName, string[] eventData)
        {
            return Parse(timestamp, eventName, eventData, new SpellAuraDose());
        }

        public SpellAuraDose Parse(DateTime timestamp, string eventName, string[] eventData, SpellAuraDose obj)
        {
            obj = (SpellAuraDose)base.Parse(timestamp, eventName, eventData, obj);
            obj.AuraType = (BuffType)Enum.Parse(typeof(BuffType), eventData[Indexes.SPELL_AURA_APPLIED_DOSE.AuraType], true);
            obj.AuraDoeseAdded = eventData[Indexes.SPELL_AURA_APPLIED_DOSE.AuraDosesAdded].ToInt();

            return obj;
        }
    }
}
