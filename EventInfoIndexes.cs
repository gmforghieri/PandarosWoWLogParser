using System;
using System.Reflection;

namespace PandarosWoWLogParser.Indexes
{
    public class UnitKeys
    {
        public const int SourceGUID = 0;
        public const int SourceName = 1;
        public const int SourceFlags = 2;
        public const int DestGUID = 3;
        public const int DestName = 4;
        public const int DestFlags = 5;
    }

    public class SPELL_CAST_START
    {
        public const int CastSpellId = 6;
        public const int CastSpellName = 7;
        public const int CastSpellSchool = 8;
    }

    public class ENCHANT
    {
        public const int SpellName = 6;
        public const int ItemID = 7;
        public const int ItemName = 8;
    }

    public class SPELL_CAST_FAILED
    {
        public const int CastFailedReason = 8;
    }

    public class SPELL_ENERGIZE
    {
        public const int EneryAmount = 9;
        public const int PowerType = 10;
    }

    public class SPELL_SUMMON
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
    }

    public class SPELL_AURA_APPLIED
    {
        public const int AuraType = 9;
    }

    public class SPELL_AURA_APPLIED_DOSE
    {
        public const int AuraType = 9;
        public const int AuraDosesAdded = 10;
    }

    public class SPELL_AURA_BROKEN_SPELL
    {
        public const int ExtraSpellID = 8;
        public const int ExtraSpellName = 9;
        public const int ExtraSchool = 10;
        public const int AuraType = 11;
    }

    public class SPELL_MISSED
    {
        public const int MissedType = 9;
    }

    public class SPELL_DAMAGE
    {
        public const int Amount = 9;
        public const int Overkill = 10;
        public const int School = 11;
        public const int Resisted = 12;
        public const int Blocked = 13;
        public const int Absorbed = 14;
        public const int Critical = 15;
        public const int Glancing = 16;
        public const int Crushing = 17;
    }

    public class SPELL_PERIODIC_DAMAGE
    {
        public const int Amount = 9;
        public const int Overkill = 10;
        public const int School = 11;
        public const int Resisted = 12;
        public const int Blocked = 13;
        public const int Absorbed = 14;
        public const int Critical = 15;
        public const int Glancing = 16;
        public const int Crushing = 17;
    }

    public class SPELL_PERIODIC_MISSED
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
        public const int MissedReason = 11;
    }

    public class SPELL_DRAIN
    {
        public const int DrainAmount = 8;
        public const int PowerType = 9;
        public const int ExtraDrainAmount = 10;
    }

    public class SPELL_HEAL 
    {
        public const int HealAmount = 8;
        public const int Overhealing = 9;
        public const int Absorbed = 10;
        public const int Critical = 11;
    }

    public class SPELL_INTERRUPT
    {
        public const int ExtraSpellId = 9;
        public const int ExtraSpellName = 10;
        public const int ExtraSchool = 11;
    }

    public class SPELL_DISPEL
    {
        public const int ExtraSpellId = 9;
        public const int ExtraSpellName = 10;
        public const int ExtraSchool = 11;
        public const int AuraType = 12;
    }

    public class SWING_DAMAGE
    {
        public const int Amount = 6;
        public const int Overkill = 7;
        public const int School = 8;
        public const int Resisted = 9;
        public const int Blocked = 10;
        public const int Absorbed = 11;
        public const int Critical = 12;
        public const int Glancing = 13;
        public const int Crushing = 14;
    }

    public class SWING_MISSED
    {
        public const int MissedReason = 6;
    }

    public class ENVIRONMENTAL_DAMAGE
    {
        public const int EnvironmentalType = 6;
        public const int Amount = 7;
        public const int Overkill = 8;
        public const int School = 9;
        public const int Resisted = 10;
        public const int Blocked = 11;
        public const int Absorbed = 12;
        public const int Critical = 13;
        public const int Glancing = 14;
        public const int Crushing = 15;
    }
}
