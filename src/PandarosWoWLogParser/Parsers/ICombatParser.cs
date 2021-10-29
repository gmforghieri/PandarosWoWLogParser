using System;
using System.Collections.Generic;
using System.Text;
using PandarosWoWLogParser.Models;

namespace PandarosWoWLogParser.Parsers
{
    public interface ICombatParser<T> where T : CombatEventBase
    {
        public T Parse(DateTime timestamp, string eventName, string[] eventData);
    }
}
