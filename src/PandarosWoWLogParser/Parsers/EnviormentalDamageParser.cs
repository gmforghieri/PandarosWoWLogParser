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
            obj.Damage = eventData[Indexes.ENVIRONMENTAL_DAMAGE.Amount].ToInt();
            obj.Overkill = eventData[Indexes.ENVIRONMENTAL_DAMAGE.Overkill].ToInt();
            obj.DamageSchool = eventData[Indexes.ENVIRONMENTAL_DAMAGE.School].ToSpellSchool();
            obj.Resisted = eventData[Indexes.ENVIRONMENTAL_DAMAGE.Resisted].ToInt();
            obj.Blocked = eventData[Indexes.ENVIRONMENTAL_DAMAGE.Blocked].ToInt();
            obj.Absorbed = eventData[Indexes.ENVIRONMENTAL_DAMAGE.Absorbed].ToInt();
            obj.Critical = eventData[Indexes.ENVIRONMENTAL_DAMAGE.Critical].ToBool();
            obj.Glancing = eventData[Indexes.ENVIRONMENTAL_DAMAGE.Glancing].ToBool();
            obj.Crushing = eventData[Indexes.ENVIRONMENTAL_DAMAGE.Crushing].ToBool();
            return obj;
        }
    }
}
