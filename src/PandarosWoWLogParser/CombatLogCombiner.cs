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
            Dictionary<string, Dictionary<long, InternalLogEntry>> masterLog = new Dictionary<string, Dictionary<long, InternalLogEntry>>();
            var minTime = -20000000;
            var maxTime = 20000000;

            foreach (var file in Directory.GetFiles(folderPath))
            {
                FileInfo fileToParse = new FileInfo(file);
                long newEvents = 0;
                long existingEvents = 0;
                var timezone = TimeZoneInfo.Local;
                var endindex = fileToParse.Name.IndexOf(']');
                var offset = "";

                if (endindex != -1)
                {
                    var startindex = fileToParse.Name.IndexOf('[') + 1;
                    var tzString = fileToParse.Name.Substring(startindex, endindex - startindex);
                    timezone = TimeZoneInfo.FindSystemTimeZoneById(tzString);

                    if (timezone.BaseUtcOffset.Hours >= 0)
                        offset += "+" + timezone.BaseUtcOffset.Hours.ToString().PadLeft(2, '0');
                    else
                        offset += "-" + (timezone.BaseUtcOffset.Hours * -1).ToString().PadLeft(2, '0');

                    offset += ":00";
                }

                using (FileStream fs = new FileStream(fileToParse.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();

                            if (string.IsNullOrWhiteSpace(line))
                                continue;

                            var evt = ParseLine(line, offset, timezone);

                            if (evt == null)
                                continue;

                            // check if event exisits already
                            if (!masterLog.TryGetValue(evt.Log, out var evtDic))
                            {
                                evtDic = new Dictionary<long, InternalLogEntry>();
                                masterLog[evt.Log] = evtDic;
                            }

                            var matchedEvents = evtDic.Where(kvp => kvp.Key - evt.Time.Ticks <= maxTime && kvp.Key - evt.Time.Ticks >= minTime);

                            if (matchedEvents.Any())
                            {
                                existingEvents++;
                                continue;
                            }


                            // if two events are in the log at the exact log time, we can add a tick to make it unique
                            while (evtDic.ContainsKey(evt.Time.Ticks))
                                evt.Time = evt.Time.AddTicks(1);

                            evtDic[evt.Time.Ticks] = evt;
                            newEvents++;
                        }

                    }
                }

                _logger.Log($"{fileToParse.Name} file had {newEvents.ToString("N")} new events and {existingEvents.ToString("N")} exisiting");
            }

            FileInfo outputLog = new FileInfo(Path.Combine(folderPath, "CombinedCombatLog-" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".log"));
            Dictionary<long, InternalLogEntry> newLog = new Dictionary<long, InternalLogEntry>();

            foreach (var line in masterLog.Values)
                foreach(var ts in line.Values)
                {
                    while (newLog.ContainsKey(ts.Time.Ticks))
                        ts.Time = ts.Time.AddTicks(1);

                    newLog[ts.Time.Ticks] = ts;
                }

            using (FileStream fs = new FileStream(outputLog.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamWriter sr = new StreamWriter(fs))
                {
                    foreach (var line in newLog.OrderBy(kvp => kvp.Key))
                    {
                        sr.WriteLine($"{line.Value.Time.ToString("MM/dd HH:mm:ss.fff")}  {line.Value.Log}");
                    }
                }
            }
        }


        private InternalLogEntry ParseLine(string line, string offset, TimeZoneInfo timeZoneInfo)
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


            string dt = $"{DateTime.Now.Year}-{month}-{day}T{hour}:{minute}:{second}.{millisecond.ToString().PadRight(7, '0')}{offset}";
            DateTime time = DateTime.Parse(dt);

            return new InternalLogEntry(time, data);
        }
    }
}
