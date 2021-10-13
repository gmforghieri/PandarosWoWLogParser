using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Models
{
    public class SpellDrain : SpellBase
    {
        public int DrainAmount { get; set; }
        public PowerType PowerType { get; set; }
        public int ExtraDrainAmount { get; set; }
    }
}
