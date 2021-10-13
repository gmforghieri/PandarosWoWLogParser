using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Parsers
{
    public class SpellAuraParser : SpellParser, ICombatParser<SpellAura>
    {
        public new SpellAura Parse(DateTime timestamp, string eventName, string[] eventData)
        {
            return Parse(timestamp, eventName, eventData, new SpellAura());
        }

        public SpellAura Parse(DateTime timestamp, string eventName, string[] eventData, SpellAura obj)
        {
            obj = (SpellAura)base.Parse(timestamp, eventName, eventData, obj);
            obj.AuraType = eventData[Indexes.SPELL_AURA_APPLIED.AuraType];

            return obj;
        }
    }
}
