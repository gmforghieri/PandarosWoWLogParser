using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Models
{
    public class SpellMissed : SpellBase, IMissed
    {
        public MissType MissType { get; set; }
    }
}
