using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Parsers
{
    public class SpellInterruptParser : SpellParser, ICombatParser<SpellInterrupt>
    {
        public new SpellInterrupt Parse(DateTime timestamp, string eventName, string[] eventData)
        {
            return Parse(timestamp, eventName, eventData, new SpellInterrupt());
        }

        public SpellInterrupt Parse(DateTime timestamp, string eventName, string[] eventData, SpellInterrupt obj)
        {
            obj = (SpellInterrupt)base.Parse(timestamp, eventName, eventData, obj);
            obj.ExtraSpellId = eventData[Indexes.SPELL_INTERRUPT.ExtraSpellId].ToInt();
            obj.ExtraSpellName = eventData[Indexes.SPELL_INTERRUPT.ExtraSpellName];
            obj.ExtraSchool = eventData[Indexes.SPELL_INTERRUPT.ExtraSchool].ToSpellSchool();

            return obj;
        }
    }
}
