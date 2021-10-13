using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Models
{
    public class MonitoredZone
    {
        public string ZoneName { get; set; }
        public List<string> MonitoredFights { get; set; }
    }
}
