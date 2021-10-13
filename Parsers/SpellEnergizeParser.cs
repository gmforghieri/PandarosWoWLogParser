using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Parsers
{
    public class SpellEnergizeParser : SpellParser, ICombatParser<SpellEnergize>
    {
        public new SpellEnergize Parse(DateTime timestamp, string eventName, string[] eventData)
        {
            return Parse(timestamp, eventName, eventData, new SpellEnergize());
        }

        public SpellEnergize Parse(DateTime timestamp, string eventName, string[] eventData, SpellEnergize obj)
        {
            obj = (SpellEnergize)base.Parse(timestamp, eventName, eventData, obj);
            obj.EneryAmount = eventData[EventInfo.SPELL_ENERGIZE.EneryAmount].ToInt();
            obj.PowerType = eventData[EventInfo.SPELL_ENERGIZE.PowerType].ToPowerType();

            return obj;
        }
    }
}
