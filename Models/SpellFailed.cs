using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Models
{
    public class SpellFailed : SpellBase, ISpellFailed
    {
        public string FailedType { get; set; }
    }
}
