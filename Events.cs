using System.Collections.Generic;

namespace PandarosWoWLogParser
{
    public static class Events
    {
        public const string ENCOUNTER_START = "ENCOUNTER_START";
        public const string ENCOUNTER_END = "ENCOUNTER_END";
        public const string SPELL_CAST_SUCCESS = "SPELL_CAST_SUCCESS";
        public const string SPELL_CAST_START = "SPELL_CAST_START";
        public const string SPELL_CAST_FAILED = "SPELL_CAST_FAILED";
        public const string SPELL_ENERGIZE = "SPELL_ENERGIZE";
        public const string SPELL_PERIODIC_ENERGIZE = "SPELL_PERIODIC_ENERGIZE";
        public const string SPELL_SUMMON = "SPELL_SUMMON";
        public const string SPELL_AURA_APPLIED = "SPELL_AURA_APPLIED";
        public const string SPELL_AURA_APPLIED_DOSE = "SPELL_AURA_APPLIED_DOSE";
        public const string SPELL_AURA_REMOVED = "SPELL_AURA_REMOVED";
        public const string SPELL_AURA_REFRESH = "SPELL_AURA_REFRESH";
        public const string SPELL_AURA_REMOVED_DOSE = "SPELL_AURA_REMOVED_DOSE";
        public const string SPELL_AURA_BROKEN_SPELL = "SPELL_AURA_BROKEN_SPELL";
        public const string SPELL_MISSED = "SPELL_MISSED";
        public const string SPELL_DAMAGE = "SPELL_DAMAGE";
        public const string SPELL_PERIODIC_DAMAGE = "SPELL_PERIODIC_DAMAGE";
        public const string SPELL_PERIODIC_MISSED = "SPELL_PERIODIC_MISSED";
        public const string SPELL_ABSORBED = "SPELL_ABSORBED";
        public const string SPELL_HEAL = "SPELL_HEAL";
        public const string SPELL_PERIODIC_HEAL = "SPELL_PERIODIC_HEAL";
        public const string SPELL_CREATE = "SPELL_CREATE";
        public const string SPELL_INTERRUPT = "SPELL_INTERRUPT";
        public const string SPELL_RESURRECT = "SPELL_RESURRECT";
        public const string SPELL_INSTAKILL = "SPELL_INSTAKILL";
        public const string RANGE_DAMAGE = "RANGE_DAMAGE";
        public const string SWING_DAMAGE = "SWING_DAMAGE";
        public const string SWING_DAMAGE_LANDED = "SWING_DAMAGE_LANDED";
        public const string SWING_MISSED = "SWING_MISSED";
        public const string UNIT_DIED = "UNIT_DIED";
        public const string UNIT_DESTROYED = "UNIT_DESTROYED";
        public const string PARTY_KILL = "PARTY_KILL";
        public const string ENVIRONMENTAL_DAMAGE = "ENVIRONMENTAL_DAMAGE";
    }
}
