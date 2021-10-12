using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser
{
    /// <summary>
    ///     https://wowpedia.fandom.com/wiki/COMBAT_LOG_EVENT#Parameter_Values
    /// </summary>
    public enum SpellSchool
    {
        Physical = 0x01,
        Holy = 0x02,
        Fire = 0x04,
        Nature = 0x08,
        Frost = 0x10,
        Shadow = 0x20,
        Arcane = 0x40,
        Holystrike = 0x03,
        Flamestrike = 0x05,
        Holyfire = 0x06,
        Stormstrike = 0x09,
        Holystorm = 0x0A,
        Firestorm = 0x0C,
        Froststrike = 0x11,
        Holyfrost = 0x12,
        Frostfire = 0x14,
        Froststorm = 0x18,
        Shadowstrike = 0x21,
        Twilight = 0x22,
        Shadowflame = 0x24,
        Plague = 0x28,
        Shadowfrost = 0x30,
        Spellstrike = 0x41,
        Divine = 0x42,
        Spellfire = 0x44,
        Spellstorm = 0x48,
        Spellfrost = 0x50,
        Spellshadow = 0x60,
        Elemental = 0x1C,
        Chromatic = 0x7C,
        Magic = 0x7E,
        Chaos = 0x7F
    }
}
