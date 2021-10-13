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
            obj.Damage = eventData[Indexes.SPELL_PERIODIC_DAMAGE.Amount].ToInt();
            obj.Overkill = eventData[Indexes.SPELL_PERIODIC_DAMAGE.Overkill].ToInt();
            obj.DamageSchool = eventData[Indexes.SPELL_PERIODIC_DAMAGE.School].ToSpellSchool();
            obj.Resisted = eventData[Indexes.SPELL_PERIODIC_DAMAGE.Resisted].ToInt();
            obj.Blocked = eventData[Indexes.SPELL_PERIODIC_DAMAGE.Blocked].ToInt();
            obj.Absorbed = eventData[Indexes.SPELL_PERIODIC_DAMAGE.Absorbed].ToInt();
            obj.Critical = eventData[Indexes.SPELL_PERIODIC_DAMAGE.Critical].ToBool();
            obj.Glancing = eventData[Indexes.SPELL_PERIODIC_DAMAGE.Glancing].ToBool();
            obj.Crushing = eventData[Indexes.SPELL_PERIODIC_DAMAGE.Crushing].ToBool();
            return obj;
        }
    }
}
