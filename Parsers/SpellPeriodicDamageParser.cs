using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Parsers
{
    public class SpellPeriodicDamageParser : SpellParser, ICombatParser<SpellPeriodicDamage>
    {
        public new SpellPeriodicDamage Parse(DateTime timestamp, string eventName, string[] eventData)
        {
            return Parse(timestamp, eventName, eventData, new SpellPeriodicDamage());
        }

        public SpellPeriodicDamage Parse(DateTime timestamp, string eventName, string[] eventData, SpellPeriodicDamage obj)
        {
            obj = (SpellPeriodicDamage)base.Parse(timestamp, eventName, eventData, obj);
            obj.Damage = eventData[EventInfo.SPELL_PERIODIC_DAMAGE.Amount].ToInt();
            obj.Overkill = eventData[EventInfo.SPELL_PERIODIC_DAMAGE.Overkill].ToInt();
            obj.DamageSchool = eventData[EventInfo.SPELL_PERIODIC_DAMAGE.School].ToSpellSchool();
            obj.Resisted = eventData[EventInfo.SPELL_PERIODIC_DAMAGE.Resisted].ToInt();
            obj.Blocked = eventData[EventInfo.SPELL_PERIODIC_DAMAGE.Blocked].ToInt();
            obj.Absorbed = eventData[EventInfo.SPELL_PERIODIC_DAMAGE.Absorbed].ToInt();
            obj.Critical = eventData[EventInfo.SPELL_PERIODIC_DAMAGE.Critical].ToBool();
            obj.Glancing = eventData[EventInfo.SPELL_PERIODIC_DAMAGE.Glancing].ToBool();
            obj.Crushing = eventData[EventInfo.SPELL_PERIODIC_DAMAGE.Crushing].ToBool();
            return obj;
        }
    }
}
