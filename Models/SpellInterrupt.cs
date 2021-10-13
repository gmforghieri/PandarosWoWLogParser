using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Models
{
    public class SpellInterrupt : SpellBase
    {
        public int ExtraSpellId { get; set; }
        public string ExtraSpellName { get; set; }
        public SpellSchool ExtraSchool { get; set; }
    }
}
