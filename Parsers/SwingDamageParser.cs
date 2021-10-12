using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Parsers
{
    public class SwingDamageParser : SwingParser, ICombatParser<SwingDamage>
    {
        public new SwingDamage Parse(DateTime timestamp, string eventName, string[] eventData)
        {
            return Parse(timestamp, eventName, eventData, new SwingDamage());
        }

        public SwingDamage Parse(DateTime timestamp, string eventName, string[] eventData, SwingDamage obj)
        {
            obj = (SwingDamage)base.Parse(timestamp, eventName, eventData, obj);
            obj.Damage = eventData[EventInfo.SWING_DAMAGE.Amount].ToInt();
            obj.Overkill = eventData[EventInfo.SWING_DAMAGE.Overkill].ToInt();
            obj.School = eventData[EventInfo.SWING_DAMAGE.School].ToSpellSchool();
            obj.Resisted = eventData[EventInfo.SWING_DAMAGE.Resisted].ToInt();
            obj.Blocked = eventData[EventInfo.SWING_DAMAGE.Blocked].ToInt();
            obj.Absorbed = eventData[EventInfo.SWING_DAMAGE.Absorbed].ToInt();
            obj.Critical = eventData[EventInfo.SWING_DAMAGE.Critical].ToBool();
            obj.Glancing = eventData[EventInfo.SWING_DAMAGE.Glancing].ToBool();
            obj.Crushing = eventData[EventInfo.SWING_DAMAGE.Crushing].ToBool();
            return obj;
        }
    }
}
