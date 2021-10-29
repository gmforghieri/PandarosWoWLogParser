using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Models
{
    public class SpellHeal : SpellBase, ISpellHeal
    {
        public int HealAmount { get; set; }
        public int Overhealing { get; set; }
        public int Absorbed { get; set; }
        public bool Critical { get; set; }
    }
}
