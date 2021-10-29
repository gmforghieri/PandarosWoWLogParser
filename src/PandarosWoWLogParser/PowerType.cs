using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser
{
    /// <summary>
    ///     https://wowpedia.fandom.com/wiki/Enum.PowerType 
    /// </summary>
    public enum PowerType
    {
        Health = -2,
        Mana = 0,
        Rage = 1,
        Focus = 2,
        Energy = 3,
        Runes = 5,
        RunicPower = 6,
        SoulShard = 7,
        Eclipse = 8,
        HolyPower = 9,
        AlternatePower = 10,
        Chi = 12,
        ShadowOrbs = 13,
        BurningEmbers = 14,
        DemonicFury = 15
    }
}
