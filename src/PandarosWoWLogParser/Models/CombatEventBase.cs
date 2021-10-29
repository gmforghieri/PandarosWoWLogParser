using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Models
{
    public class CombatEventBase : ICombatEvent
    {
        public DateTime Timestamp { get; set; }
        public string EventName { get; set; }
        public string SourceGuid { get; set; }
        public string SourceName { get; set; }
        public UnitFlags SourceFlags { get; set; }
        public string DestGuid { get; set; }
        public string DestName { get; set; }
        public UnitFlags DestFlags { get; set; }
        public string[] EventParameters { get; set; }
    }
}
