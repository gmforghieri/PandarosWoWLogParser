using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Parsers
{
    public class EnviormentalDamageParser : EnviormentParser, ICombatParser<EnviormentalDamage>
    {
        public new virtual EnviormentalDamage Parse(DateTime timestamp, string eventName, string[] eventData)
        {
            return Parse(timestamp, eventName, eventData, new EnviormentalDamage());
        }

        public virtual EnviormentalDamage Parse(DateTime timestamp, string eventName, string[] eventData, EnviormentalDamage obj)
        {
            obj = (EnviormentalDamage)base.Parse(timestamp, eventName, eventData, obj);
            obj.Damage = eventData[EventInfo.ENVIRONMENTAL_DAMAGE.Amount].ToInt();
            obj.Overkill = eventData[EventInfo.ENVIRONMENTAL_DAMAGE.Overkill].ToInt();
            obj.DamageSchool = eventData[EventInfo.ENVIRONMENTAL_DAMAGE.School].ToSpellSchool();
            obj.Resisted = eventData[EventInfo.ENVIRONMENTAL_DAMAGE.Resisted].ToInt();
            obj.Blocked = eventData[EventInfo.ENVIRONMENTAL_DAMAGE.Blocked].ToInt();
            obj.Absorbed = eventData[EventInfo.ENVIRONMENTAL_DAMAGE.Absorbed].ToInt();
            obj.Critical = eventData[EventInfo.ENVIRONMENTAL_DAMAGE.Critical].ToBool();
            obj.Glancing = eventData[EventInfo.ENVIRONMENTAL_DAMAGE.Glancing].ToBool();
            obj.Crushing = eventData[EventInfo.ENVIRONMENTAL_DAMAGE.Crushing].ToBool();
            return obj;
        }
    }
}
