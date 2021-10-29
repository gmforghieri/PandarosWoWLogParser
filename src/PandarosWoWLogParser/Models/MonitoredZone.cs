using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Models
{
    public class MonitoredZone
    {
        public string ZoneName { get; set; }
        public Dictionary<string, List<string>> MonitoredFights { get; set; } = new Dictionary<string, List<string>>();
    }
}
