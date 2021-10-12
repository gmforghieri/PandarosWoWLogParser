using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Models
{
    public class SpellPeriodicDamage : SpellBase
    {
        public int Damage { get; set; }
        public int Overkill { get; set; }
        public SpellSchool DamageSchool { get; set; }
        public int Resisted { get; set; }
        public int Blocked { get; set; }
        public int Absorbed { get; set; }
        public bool Critical { get; set; }
        public bool Glancing { get; set; }
        public bool Crushing { get; set; }
    }
}
