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
            obj.Damage = eventData[Indexes.SWING_DAMAGE.Amount].ToInt();
            obj.Overkill = eventData[Indexes.SWING_DAMAGE.Overkill].ToInt();
            obj.School = eventData[Indexes.SWING_DAMAGE.School].ToSpellSchool();
            obj.Resisted = eventData[Indexes.SWING_DAMAGE.Resisted].ToInt();
            obj.Blocked = eventData[Indexes.SWING_DAMAGE.Blocked].ToInt();
            obj.Absorbed = eventData[Indexes.SWING_DAMAGE.Absorbed].ToInt();
            obj.Critical = eventData[Indexes.SWING_DAMAGE.Critical].ToBool();
            obj.Glancing = eventData[Indexes.SWING_DAMAGE.Glancing].ToBool();
            obj.Crushing = eventData[Indexes.SWING_DAMAGE.Crushing].ToBool();
            return obj;
        }
    }
}
