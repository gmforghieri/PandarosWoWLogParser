using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Parsers
{
    public class SpellDispelParser : SpellParser, ICombatParser<SpellDispel>
    {
        public new SpellDispel Parse(DateTime timestamp, string eventName, string[] eventData)
        {
            return Parse(timestamp, eventName, eventData, new SpellDispel());
        }

        public SpellDispel Parse(DateTime timestamp, string eventName, string[] eventData, SpellDispel obj)
        {
            obj = (SpellDispel)base.Parse(timestamp, eventName, eventData, obj);
            obj.ExtraSpellId = eventData[Indexes.SPELL_DISPEL.ExtraSpellId].ToInt();
            obj.ExtraSpellName = eventData[Indexes.SPELL_DISPEL.ExtraSpellName];
            obj.ExtraSchool = eventData[Indexes.SPELL_DISPEL.ExtraSchool].ToSpellSchool();
            obj.AuraType = eventData[Indexes.SPELL_DISPEL.AuraType];
            return obj;
        }
    }
}
