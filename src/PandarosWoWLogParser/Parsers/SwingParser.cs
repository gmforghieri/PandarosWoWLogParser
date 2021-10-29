using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Parsers
{
    public abstract class SwingParser : BaseParser, ICombatParser<SwingBase>
    {
        public new virtual SwingBase Parse(DateTime timestamp, string eventName, string[] eventData)
        {
            return Parse(timestamp, eventName, eventData, new SwingBase());
        }

        public SwingBase Parse(DateTime timestamp, string eventName, string[] eventData, SwingBase obj)
        {
            return (SwingBase)base.Parse(timestamp, eventName, eventData, obj);
        }
    }
}
