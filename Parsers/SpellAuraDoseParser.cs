using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

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
            obj.AuraType = eventData[Indexes.SPELL_AURA_APPLIED_DOSE.AuraType];
            obj.AuraDoeseAdded = eventData[Indexes.SPELL_AURA_APPLIED_DOSE.AuraDosesAdded].ToInt();

            return obj;
        }
    }
}
