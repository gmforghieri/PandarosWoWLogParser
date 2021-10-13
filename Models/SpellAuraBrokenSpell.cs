using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Models
{
    public class SpellAuraBrokenSpell : SpellBase
    {
        public int ExtraSpellID { get; set; }
        public string ExtraSpellName { get; set; }
        public SpellSchool ExtraSpellSchool { get; set; }
        public string AuraType { get; set; }
    }
}
