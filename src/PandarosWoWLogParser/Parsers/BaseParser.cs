using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Parsers
{
    public class BaseParser : ICombatParser<CombatEventBase>
    {
        public virtual CombatEventBase Parse(DateTime timestamp, string eventName, string[] eventData)
        {
            return Parse(timestamp, eventName, eventData, new CombatEventBase());
        }

        public virtual CombatEventBase Parse(DateTime timestamp, string eventName, string[] eventData, CombatEventBase obj)
        {
            obj.Timestamp = timestamp;
            obj.EventName = eventName;
            obj.EventParameters = eventData;
            obj.SourceGuid = eventData[Indexes.UnitKeys.SourceGUID];
            obj.SourceName = eventData[Indexes.UnitKeys.SourceName];
            obj.SourceFlags = eventData[Indexes.UnitKeys.SourceFlags].ToUnitFlags();
            obj.DestGuid = eventData[Indexes.UnitKeys.DestGUID];
            obj.DestName = eventData[Indexes.UnitKeys.DestName];
            obj.DestFlags = eventData[Indexes.UnitKeys.DestFlags].ToUnitFlags();
            return obj;
        }
    }
}
