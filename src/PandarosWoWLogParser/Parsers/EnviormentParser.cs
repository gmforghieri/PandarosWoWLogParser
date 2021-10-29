using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Parsers
{
    public class EnviormentParser : BaseParser, ICombatParser<EnviormentalBase>
    {
        public new virtual EnviormentalBase Parse(DateTime timestamp, string eventName, string[] eventData)
        {
            return Parse(timestamp, eventName, eventData, new EnviormentalBase());
        }

        public virtual EnviormentalBase Parse(DateTime timestamp, string eventName, string[] eventData, EnviormentalBase obj)
        {
            obj = (EnviormentalBase)base.Parse(timestamp, eventName, eventData, obj);
            obj.EnvironmentalType = eventData[Indexes.ENVIRONMENTAL_DAMAGE.EnvironmentalType];
            return obj;
        }
    }
}
