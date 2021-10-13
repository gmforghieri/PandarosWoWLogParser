using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Parsers
{
    public class ParserFactory : IParserFactory
    {
        public Dictionary<string, Func<DateTime, string, string[], CombatEventBase>> Parsers { get; set; } = new Dictionary<string, Func<DateTime, string, string[], CombatEventBase>>();


        public ParserFactory(ICombatParser<SpellDamage> spelldamageParser,
                               ICombatParser<SpellBase> spellParser,
                               ICombatParser<SpellFailed> spellFailedParser,
                               ICombatParser<SwingDamage> swingDamageParser,
                               ICombatParser<SpellAura> spellAuraParser,
                               ICombatParser<SpellAuraDose> spellAuraDoseParser,
                               ICombatParser<SpellAuraBrokenSpell> spellAuraBrokenSpellParser,
                               ICombatParser<SpellMissed> spellmissedParser,
                               ICombatParser<SwingMissed> swingmissedParser,
                               ICombatParser<SpellHeal> spellHealParser,
                               ICombatParser<CombatEventBase> combatEventBaseParser,
                               ICombatParser<EnviormentalDamage> enviormentalDamageParser,
                               ICombatParser<SpellDispel> spellDispelParser,
                               ICombatParser<SpellInterrupt> spellInterruptParser,
                               ICombatParser<SpellDrain> spellDrainParser,
                               ICombatParser<Enchant> enchantParser,
                               ICombatParser<SpellEnergize> spellEnergizeParser)
        {
            Parsers.Add(LogEvents.SWING_DAMAGE, swingDamageParser.Parse);
            Parsers.Add(LogEvents.SWING_MISSED, swingmissedParser.Parse);
            Parsers.Add(LogEvents.DAMAGE_SHIELD, spelldamageParser.Parse);
            Parsers.Add(LogEvents.SPELL_DAMAGE, spelldamageParser.Parse);
            Parsers.Add(LogEvents.RANGE_DAMAGE, spelldamageParser.Parse);
            Parsers.Add(LogEvents.SPELL_PERIODIC_DAMAGE, spelldamageParser.Parse);
            Parsers.Add(LogEvents.ENVIRONMENTAL_DAMAGE, enviormentalDamageParser.Parse);
            Parsers.Add(LogEvents.SPELL_CAST_START, spellParser.Parse);
            Parsers.Add(LogEvents.SPELL_CAST_SUCCESS, spellParser.Parse);
            Parsers.Add(LogEvents.SPELL_SUMMON, spellParser.Parse);
            Parsers.Add(LogEvents.SPELL_CREATE, spellParser.Parse);
            Parsers.Add(LogEvents.SPELL_RESURRECT, spellParser.Parse);
            Parsers.Add(LogEvents.SPELL_ABSORBED, spellParser.Parse);
            Parsers.Add(LogEvents.SPELL_INSTAKILL, spellParser.Parse);
            Parsers.Add(LogEvents.SPELL_DURABILITY_DAMAGE, spellParser.Parse);
            Parsers.Add(LogEvents.SPELL_DURABILITY_DAMAGE_ALL, spellParser.Parse);
            Parsers.Add(LogEvents.SPELL_CAST_FAILED, spellFailedParser.Parse);
            Parsers.Add(LogEvents.SPELL_PERIODIC_ENERGIZE, spellEnergizeParser.Parse);
            Parsers.Add(LogEvents.SPELL_ENERGIZE, spellEnergizeParser.Parse);
            Parsers.Add(LogEvents.SPELL_AURA_APPLIED, spellAuraParser.Parse);
            Parsers.Add(LogEvents.SPELL_AURA_REMOVED, spellAuraParser.Parse);
            Parsers.Add(LogEvents.SPELL_AURA_REFRESH, spellAuraParser.Parse);
            Parsers.Add(LogEvents.SPELL_AURA_BROKEN, spellAuraParser.Parse);
            Parsers.Add(LogEvents.SPELL_AURA_APPLIED_DOSE, spellAuraDoseParser.Parse);
            Parsers.Add(LogEvents.SPELL_AURA_REMOVED_DOSE, spellAuraDoseParser.Parse);
            Parsers.Add(LogEvents.SPELL_PERIODIC_MISSED, spellmissedParser.Parse);
            Parsers.Add(LogEvents.SPELL_MISSED, spellmissedParser.Parse);
            Parsers.Add(LogEvents.DAMAGE_SHIELD_MISSED, spellmissedParser.Parse);
            Parsers.Add(LogEvents.RANGE_MISSED, spellmissedParser.Parse);
            Parsers.Add(LogEvents.SPELL_HEAL, spellHealParser.Parse);
            Parsers.Add(LogEvents.SPELL_PERIODIC_HEAL, spellHealParser.Parse);
            Parsers.Add(LogEvents.PARTY_KILL, combatEventBaseParser.Parse);
            Parsers.Add(LogEvents.UNIT_DESTROYED, combatEventBaseParser.Parse);
            Parsers.Add(LogEvents.UNIT_DIED, combatEventBaseParser.Parse);
            Parsers.Add(LogEvents.SPELL_DISPEL, spellDispelParser.Parse);
            Parsers.Add(LogEvents.SPELL_STOLEN, spellDispelParser.Parse);
            Parsers.Add(LogEvents.SPELL_INTERRUPT, spellInterruptParser.Parse);
            Parsers.Add(LogEvents.SPELL_DISPEL_FAILED, spellInterruptParser.Parse);
            Parsers.Add(LogEvents.SPELL_DRAIN, spellDrainParser.Parse);
            Parsers.Add(LogEvents.SPELL_LEECH, spellDrainParser.Parse);
            Parsers.Add(LogEvents.ENCHANT_REMOVED, enchantParser.Parse);
            Parsers.Add(LogEvents.ENCHANT_APPLIED, enchantParser.Parse);
        }

        public CombatEventBase Parse(DateTime date, string eventName, string[] data)
        {
            if (Parsers.TryGetValue(eventName, out var parse))
                return parse(date, eventName, data);
            else
                return null;
        }
    }
}
