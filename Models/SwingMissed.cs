using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Models
{
    public class SwingMissed : SwingBase, ISwingMissed
    {
        public string MissedReason { get; set; }
    }
}
