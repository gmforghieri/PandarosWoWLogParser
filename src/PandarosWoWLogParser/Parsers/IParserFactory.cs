using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;

namespace PandarosWoWLogParser.Parsers
{
    public interface IParserFactory
    {
        Dictionary<string, Func<DateTime, string, string[], CombatEventBase>> Parsers { get; set; }

        CombatEventBase Parse(DateTime date, string eventName, string[] data);
    }
}