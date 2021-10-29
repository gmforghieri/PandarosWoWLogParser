using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Models
{
    public class SpellBase : CombatEventBase, ISpell
    {
        public int SpellId { get; set; }
        public string SpellName { get; set; }
        public SpellSchool School { get; set; }
    }
}
