﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Models
{
    public class SpellMissed : SpellBase, ISpellMissed
    {
        public string MissType { get; set; }
        public int MissDamage { get; set; }
    }
}
