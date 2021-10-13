using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Models
{
    public class Enchant : CombatEventBase, IEnchant
    {
        public string SpellName { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
    }
}
