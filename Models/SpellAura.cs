using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Models
{
    public class SpellAura : SpellBase, ISpellAura
    {
        public string AuraType { get; set; }
    }
}
