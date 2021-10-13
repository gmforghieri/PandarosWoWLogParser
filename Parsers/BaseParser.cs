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
            obj.SourceGuid = eventData[EventInfo.UnitKeys.SourceGUID];
            obj.SourceName = eventData[EventInfo.UnitKeys.SourceName];
            obj.SourceFlags = eventData[EventInfo.UnitKeys.SourceFlags].ToUnitFlags();
            obj.DestGuid = eventData[EventInfo.UnitKeys.SourceGUID];
            obj.DestName = eventData[EventInfo.UnitKeys.DestName];
            obj.DestFlags = eventData[EventInfo.UnitKeys.DestFlags].ToUnitFlags();
            return obj;
        }
    }
}
