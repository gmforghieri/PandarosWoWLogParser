using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Parsers
{
    public class SpellHealParser : SpellParser, ICombatParser<SpellHeal>
    {
        public new SpellHeal Parse(DateTime timestamp, string eventName, string[] eventData)
        {
            return Parse(timestamp, eventName, eventData, new SpellHeal());
        }

        public SpellHeal Parse(DateTime timestamp, string eventName, string[] eventData, SpellHeal obj)
        {
            obj = (SpellHeal)base.Parse(timestamp, eventName, eventData, obj);
            obj.HealAmount = eventData[EventInfo.SPELL_HEAL.HealAmount].ToInt();
            obj.Overhealing = eventData[EventInfo.SPELL_HEAL.Overhealing].ToInt();
            obj.Absorbed = eventData[EventInfo.SPELL_HEAL.Absorbed].ToInt();
            obj.Critical = eventData[EventInfo.SPELL_HEAL.Critical].ToBool();
            return obj;
        }
    }
}
