using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Models
{
    public class SpellAuraDose : SpellBase, ISpellAuraDose, ISpellAura
    {
        public string AuraType { get; set; }
        public int AuraDoeseAdded { get; set; }
    }
}
