using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Parsers
{
    public class SpellDamageParser : SpellParser, ICombatParser<SpellDamage>
    {
        public new SpellDamage Parse(DateTime timestamp, string eventName, string[] eventData)
        {
            return Parse(timestamp, eventName, eventData, new SpellDamage());
        }

        public SpellDamage Parse(DateTime timestamp, string eventName, string[] eventData, SpellDamage obj)
        {
            obj = (SpellDamage)base.Parse(timestamp, eventName, eventData, obj);
            obj.Damage = eventData[EventInfo.SPELL_DAMAGE.Amount].ToInt();
            obj.Overkill = eventData[EventInfo.SPELL_DAMAGE.Overkill].ToInt();
            obj.DamageSchool = eventData[EventInfo.SPELL_DAMAGE.School].ToSpellSchool();
            obj.Resisted = eventData[EventInfo.SPELL_DAMAGE.Resisted].ToInt();
            obj.Blocked = eventData[EventInfo.SPELL_DAMAGE.Blocked].ToInt();
            obj.Absorbed = eventData[EventInfo.SPELL_DAMAGE.Absorbed].ToInt();
            obj.Critical = eventData[EventInfo.SPELL_DAMAGE.Critical].ToBool();
            obj.Glancing = eventData[EventInfo.SPELL_DAMAGE.Glancing].ToBool();
            obj.Crushing = eventData[EventInfo.SPELL_DAMAGE.Crushing].ToBool();
            return obj;
        }
    }
}
