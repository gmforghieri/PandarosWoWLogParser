using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Models
{
    public class SwingMissed : SwingBase, IMissed
    {
        public MissType MissType { get; set; }
    }
}
