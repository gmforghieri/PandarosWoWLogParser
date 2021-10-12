﻿using System;
using System.Reflection;

namespace PandarosWoWLogParser.EventInfo
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

    public class ENCOUNTER_START
    {
        public const int EncounterId = 0;
        public const int EncounterName = 1;
        public const int DifficultyId = 2;
        public const int RaidSize = 3;
        public const bool HasUnitKeys = false;
    }

    public class ENCOUNTER_END
    {
        public const int EncounterId = 0;
        public const int EncounterName = 1;
        public const int DifficultyId = 2;
        public const int RaidSize = 3;
        public const int Wiped = 4;
        public const bool HasUnitKeys = false;
    }

    public class SPELL_CAST_SUCCESS
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
        public const int TargetGUID = 11;
        public const int TargetGUIDOwner = 12;
        public const int TargetHP = 13;
        public const int TargetMaxHP = 14;
        public const int TargetAttackPower = 15;
        public const int TargetSpellPower = 16;
        public const int TargetResolve = 17;
        public const int CastSpellResource = 18;
        public const int CurrentResource = 19;
        public const int MaxResource = 20;
        public const int TargetPosX = 21;
        public const int TargetPosY = 22;
        public const int TargetItemLvl = 23;
    }

    public class SPELL_CAST_START
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
        public const bool HasUnitKeys = false;
    }

    public class SPELL_CAST_FAILED
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
        public const int CastFailedReason = 11;
    }

    public class SPELL_ENERGIZE
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
        public const int TargetGUID = 11;
        public const int TargetGUIDOwner = 12;
        public const int TargetHP = 13;
        public const int TargetMaxHP = 14;
        public const int TargetAttackPower = 15;
        public const int TargetSpellPower = 16;
        public const int TargetResolve = 17;
        public const int CastSpellResource = 18;
        public const int CurrentResource = 19;
        public const int MaxResource = 20;
        public const int TargetPosX = 21;
        public const int TargetPosY = 22;
        public const int TargetItemLvl = 23;
        public const int ResourceGain = 24;
        public const int ResourceType = 25;
    }

    public class SPELL_PERIODIC_ENERGIZE //unsure of 17
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
        public const int TargetGUID = 11;
        public const int TargetGUIDOwner = 12;
        public const int TargetHP = 13;
        public const int TargetMaxHP = 14;
        public const int TargetAttackPower = 15;
        public const int TargetSpellPower = 16;
        public const int TargetResolve = 17;
        public const int CastSpellResource = 18;
        public const int CurrentResource = 19;
        public const int MaxResource = 20;
        public const int TargetPosX = 21;
        public const int TargetPosY = 22;
        public const int TargetItemLvl = 23;
        public const int ResourceGain = 24;
        public const int ResourceType = 25;
    }

    public class SPELL_SUMMON
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
    }

    public class SPELL_AURA_APPLIED
    {
        public const int AuraSpellId = 8;
        public const int AuraSpellName = 9;
        public const int AuraSpellSchool = 10;
        public const int AuraBuffType = 11;
    }

    public class SPELL_AURA_APPLIED_DOSE
    {
        public const int AuraSpellId = 8;
        public const int AuraSpellName = 9;
        public const int AuraSpellSchool = 10;
        public const int AuraBuffType = 11;
        public const int AuraDosesAdded = 12;
    }

    public class SPELL_AURA_REMOVED
    {
        public const int AuraSpellId = 8;
        public const int AuraSpellName = 9;
        public const int AuraSpellSchool = 10;
        public const int AuraBuffType = 11;
    }

    public class SPELL_AURA_REFRESH
    {
        public const int AuraSpellId = 8;
        public const int AuraSpellName = 9;
        public const int AuraSpellSchool = 10;
        public const int AuraBuffType = 11;
    }

    public class SPELL_AURA_REMOVED_DOSE
    {
        public const int AuraSpellId = 8;
        public const int AuraSpellName = 9;
        public const int AuraSpellSchool = 10;
        public const int AuraBuffType = 11;
        public const int AuraDosesRemoved = 12;
    }

    public class SPELL_AURA_BROKEN_SPELL
    {
        public const int CastAuraSpellId = 8;
        public const int CastAuraSpellName = 9;
        public const int CastAuraSpellSchool = 10;
        public const int RemovedAuraSpellId = 11;
        public const int RemovedAuraSpellName = 12;
        public const int RemovedAuraSpellSchool = 13;
        public const int CastAuraBuffType = 14;
    }

    public class SPELL_MISSED //12-13 unknown
    {
        public const int MissedReason = 11;
    }

    public class SPELL_DAMAGE //27-33 unknown
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

    public class SPELL_PERIODIC_DAMAGE //27-33 unknown
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

    public class SPELL_PERIODIC_MISSED //12-14 unknown
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
        public const int MissedReason = 11;
    }

    public class SPELL_ABSORBED //8-15 unknown
    {
        public const bool HasUnitKeys = true;
    }

    public class SPELL_HEAL //27-28 unknown
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
        public const int TargetGUID = 11;
        public const int TargetGUIDOwner = 12;
        public const int TargetHP = 13;
        public const int TargetMaxHP = 14;
        public const int TargetAttackPower = 15;
        public const int TargetSpellPower = 16;
        public const int TargetResolve = 17;
        public const int CastSpellResource = 18;
        public const int CurrentResource = 19;
        public const int MaxResource = 20;
        public const int TargetPosX = 21;
        public const int TargetPosY = 22;
        public const int TargetItemLvl = 23;
        public const int HealAmount = 24;
        public const int Overheal = 25;
        public const int HealSpellSchool = 26;
    }

    public class SPELL_PERIODIC_HEAL //27-28 unknown
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
        public const int TargetGUID = 11;
        public const int TargetGUIDOwner = 12;
        public const int TargetHP = 13;
        public const int TargetMaxHP = 14;
        public const int TargetAttackPower = 15;
        public const int TargetSpellPower = 16;
        public const int TargetResolve = 17;
        public const int CastSpellResource = 18;
        public const int CurrentResource = 19;
        public const int MaxResource = 20;
        public const int TargetPosX = 21;
        public const int TargetPosY = 22;
        public const int TargetItemLvl = 23;
        public const int HealAmount = 24;
        public const int Overheal = 25;
        public const int HealSpellSchool = 26;
    }

    public class SPELL_CREATE
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
    }

    public class SPELL_INTERRUPT
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
        public const int InterruptedSpellId = 11;
        public const int InterruptedSpellName = 12;
        public const int InterruptedSpellSpellSchool = 13;
    }

    public class SPELL_RESURRECT
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
    }

    public class SPELL_INSTAKILL
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
    }

    public class RANGE_DAMAGE //unknown 26-33
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
        public const int TargetGUID = 11;
        public const int TargetGUIDOwner = 12;
        public const int TargetHP = 13;
        public const int TargetMaxHP = 14;
        public const int TargetAttackPower = 15;
        public const int TargetSpellPower = 16;
        public const int TargetResolve = 17;
        public const int CastSpellResource = 18;
        public const int CurrentResource = 19;
        public const int MaxResource = 20;
        public const int TargetPosX = 21;
        public const int TargetPosY = 22;
        public const int TargetItemLvl = 23;
        public const int DamageDone = 24;
        public const int Overkill = 25;
        //26 is probably damage spell school
    }

    public class SWING_DAMAGE //24-30 unknown
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

    public class SWING_MISSED //9-11 missing
    {
        public const int MissedReason = 8;
    }

    public class UNIT_DIED
    {
        public const int UnitGUID = 4;
        public const int UnitName = 5;
        public const int UnitSourceFlags = 6;
        public const int UnitSourceFlags2 = 7;
    }

    public class UNIT_DESTROYED
    {
        public const int UnitGUID = 4;
        public const int UnitName = 5;
        public const int UnitFlags = 6;
        public const int UnitFlags2 = 7;
    }

    public class PARTY_KILL
    {
        public const int FriendlyGUID = 0;
        public const int FriendlyName = 1;
        public const int FriendlyFlags = 2;
        public const int FriendlyFlags2 = 3;
        public const int EnemyGUID = 4;
        public const int EnemyName = 5;
        public const int EnemyFlags = 6;
        public const int EnemyFlags2 = 7;
    }

    public class ENVIRONMENTAL_DAMAGE //22-31 unknown
    {
        public const int EnvironmentalType = 8;
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

    public class RANGE_MISSED //12-13 unknown
    {
        public const int SpellId = 8;
        public const int SpellName = 9;
        public const int SpellSchool = 10;
        public const int MissedReason = 11;
    }
}// namespace CombatLogEvent.EventInfo