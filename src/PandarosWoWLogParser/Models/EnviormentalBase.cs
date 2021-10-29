using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Models
{
    public class EnviormentalBase : CombatEventBase, IEnviormentalBase
    {
        public string EnvironmentalType { get; set; }
    }
}
