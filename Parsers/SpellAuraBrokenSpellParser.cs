using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Parsers
{
    public class SpellAuraBrokenSpellParser : SpellParser, ICombatParser<SpellAuraBrokenSpell>
    {
        public new SpellAuraBrokenSpell Parse(DateTime timestamp, string eventName, string[] eventData)
        {
            return Parse(timestamp, eventName, eventData, new SpellAuraBrokenSpell());
        }

        public SpellAuraBrokenSpell Parse(DateTime timestamp, string eventName, string[] eventData, SpellAuraBrokenSpell obj)
        {
            obj = (SpellAuraBrokenSpell)base.Parse(timestamp, eventName, eventData, obj);
            obj.ExtraSpellID = eventData[Indexes.SPELL_AURA_BROKEN_SPELL.ExtraSpellID].ToInt();
            obj.ExtraSpellName = eventData[Indexes.SPELL_AURA_BROKEN_SPELL.ExtraSpellName];
            obj.ExtraSpellSchool = eventData[Indexes.SPELL_AURA_BROKEN_SPELL.ExtraSchool].ToSpellSchool();
            obj.AuraType = eventData[Indexes.SPELL_AURA_BROKEN_SPELL.AuraType];
            return obj;
        }
    }
}
