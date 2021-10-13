using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Parsers
{
    public class SpellDrainParser : SpellParser, ICombatParser<SpellDrain>
    {
        public new SpellDrain Parse(DateTime timestamp, string eventName, string[] eventData)
        {
            return Parse(timestamp, eventName, eventData, new SpellDrain());
        }

        public SpellDrain Parse(DateTime timestamp, string eventName, string[] eventData, SpellDrain obj)
        {
            obj = (SpellDrain)base.Parse(timestamp, eventName, eventData, obj);
            obj.DrainAmount = eventData[Indexes.SPELL_DRAIN.DrainAmount].ToInt();
            obj.PowerType = eventData[Indexes.SPELL_DRAIN.PowerType].ToPowerType();
            obj.ExtraDrainAmount = eventData[Indexes.SPELL_DRAIN.ExtraDrainAmount].ToInt();

            return obj;
        }
    }
}
