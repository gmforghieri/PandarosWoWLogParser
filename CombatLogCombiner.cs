using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PandarosWoWLogParser.Models;
using System.Threading.Tasks;
using PandarosWoWLogParser.Parsers;
using PandarosWoWLogParser.Calculators;
using PandarosWoWLogParser.FightMonitor;
using System.Linq;

namespace PandarosWoWLogParser
{
    public class CombatLogCombiner
    {
        IPandaLogger _logger;

        private class InternalLogEntry
        {
            internal DateTime Time { get; set; }
            internal string Log { get; set; }

            internal InternalLogEntry(DateTime time, string log)
            {
                Time = time;
                Log = log;
            }
        }

        public CombatLogCombiner(IPandaLogger logger)
        {
            _logger = logger;
        }

        public void ParseToEnd(string folderPath)
        {
            // key by ticks to sort later
            Dictionary<long, InternalLogEntry> masterLog = new Dictionary<long, InternalLogEntry>();
            var minTime = -2000000;
            var maxTime = 2000000;

            foreach (var file in Directory.GetFiles(folderPath))
            {
                FileInfo fileToParse = new FileInfo(file);
                long newEvents = 0;
                long existingEvents = 0;

                using (FileStream fs = new FileStream(fileToParse.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();

                            if (string.IsNullOrWhiteSpace(line))
                                continue;

                            var evt = ParseLine(line);

                            if (evt == null)
                                continue;

                            // check if event exisits already
                            var matchedEvents = masterLog.Where(kvp => kvp.Value.Log == evt.Log && kvp.Key - evt.Time.Ticks <= maxTime && kvp.Key - evt.Time.Ticks >= minTime);

                            if (matchedEvents.Any())
                            {
                                existingEvents++;
                                continue;
                            }

                            // if two events are in the log at the exact log time, we can add a tick to make it unique
                            while (masterLog.ContainsKey(evt.Time.Ticks))
                                evt.Time = evt.Time.AddTicks(1);

                            masterLog[evt.Time.Ticks] = evt;
                            newEvents++;
                        }

                    }
                }

                _logger.Log($"{fileToParse.Name} file had {newEvents.ToString("N")} new events and {existingEvents.ToString("N")} exisiting");
            }

            FileInfo outputLog = new FileInfo(Path.Combine(folderPath, "CombinedCombatLog-" + DateTime.Now.ToString() + ".log"));

            using (FileStream fs = new FileStream(outputLog.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamWriter sr = new StreamWriter(fs))
                {
                    foreach (var line in masterLog.OrderBy(kvp => kvp.Key).Reverse())
                    {
                        sr.WriteLine($"{line.Value.Time.ToString("MM/dd HH:mm:ss.fff")}  {line.Value.Log}");
                    }
                }
            }
        }


        private InternalLogEntry ParseLine(string line)
        {
            Regex r = new Regex(@"(\d{1,2})/(\d{1,2})\s(\d{2}):(\d{2}):(\d{2}).(\d{3})\s\s(.+)$"); //matches the date format used in the combat log
            Match m = r.Match(line);
            GroupCollection collection = m.Groups;

            if (collection.Count != 8)
            {
                return null;
            }

            string month = collection[1].Value;
            string day = collection[2].Value;
            string hour = collection[3].Value;
            string minute = collection[4].Value;
            string second = collection[5].Value;
            string millisecond = collection[6].Value;

            string data = collection[7].Value;
            DateTime time = new DateTime(DateTime.Now.Year, int.Parse(month), int.Parse(day), int.Parse(hour), int.Parse(minute), int.Parse(second), int.Parse(millisecond));

            return new InternalLogEntry(time, data);
        }
    }
}
