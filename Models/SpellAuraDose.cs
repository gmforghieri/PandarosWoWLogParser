using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Models
{
    public class SpellAuraDose : SpellBase
    {
        public string AuraType { get; set; }
        public int AuraDoeseAdded { get; set; }
    }
}
