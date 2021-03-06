using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Parsers
{
    public class SwingMissedParser : SpellParser, ICombatParser<SwingMissed>
    {
        public new SwingMissed Parse(DateTime timestamp, string eventName, string[] eventData)
        {
            return Parse(timestamp, eventName, eventData, new SwingMissed());
        }

        public SwingMissed Parse(DateTime timestamp, string eventName, string[] eventData, SwingMissed obj)
        {
            obj = (SwingMissed)base.Parse(timestamp, eventName, eventData, obj);
            obj.MissType = (MissType)Enum.Parse(typeof(MissType), eventData[Indexes.SWING_MISSED.MissedReason], true);
            return obj;
        }
    }
}
