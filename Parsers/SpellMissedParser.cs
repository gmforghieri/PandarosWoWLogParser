using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Parsers
{
    public class SpellMissedParser : SpellParser, ICombatParser<SpellMissed>
    {
        public new SpellMissed Parse(DateTime timestamp, string eventName, string[] eventData)
        {
            return Parse(timestamp, eventName, eventData, new SpellMissed());
        }

        public SpellMissed Parse(DateTime timestamp, string eventName, string[] eventData, SpellMissed obj)
        {
            obj = (SpellMissed)base.Parse(timestamp, eventName, eventData, obj);
            obj.MissType = eventData[EventInfo.SPELL_MISSED.MissedType];
            obj.MissDamage = eventData[EventInfo.SPELL_MISSED.MissedDamage].ToInt();
            return obj;
        }
    }
}
