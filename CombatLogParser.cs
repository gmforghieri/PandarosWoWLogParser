using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PandarosWoWLogParser.Models;
using System.Threading.Tasks;
using PandarosWoWLogParser.Parsers;
using Autofac;
using PandarosWoWLogParser.Calculators;
using PandarosWoWLogParser.FightMonitor;

namespace PandarosWoWLogParser
{
    public class CombatLogParser
    {
        public bool IsParsing { get; private set; } = false;
        public float ParseCompletionPercent { get; private set; } = 0f;
        public FileInfo FileInfo { get; private set; }

        IParserFactory _parserFactory;
        IFightMonitorFactory _fightMonitorFactory;
        IPandaLogger _logger;
        IStatsReporter _reporter;

        public CombatLogParser(IParserFactory parserFactory, IFightMonitorFactory fightMonitorFactory, IPandaLogger logger, IStatsReporter reporter)
        {
            _parserFactory = parserFactory;
            _fightMonitorFactory = fightMonitorFactory;
            _logger = logger;
            _reporter = reporter;
        }

        public int ParseToEnd(string filepath)
        {
            if (IsParsing)
                return -1;

            ParseCompletionPercent = 0f;
            IsParsing = true;
            var count = 0;

            if (File.Exists(filepath))
            {
                FileInfo = new FileInfo(filepath);
            }
            else
                throw new FileNotFoundException("Combat Log not found", filepath);

            Dictionary<string, int> unknown = new Dictionary<string, int>();
            Dictionary<string, int> eventCount = new Dictionary<string, int>();
            bool isInFight = false;
            CombatState state = new CombatState();
            MonitoredFight allFights = new MonitoredFight()
            {
                CurrentZone = new MonitoredZone()
                {
                    ZoneName = "All",
                    MonitoredFights = new Dictionary<string, List<string>>()
                },
                BossName = "All Fights in Log"
            };

            ICalculatorFactory _calculatorFactory = new CalculatorFactory(_logger, _reporter, state, allFights);

            using (FileStream fs = new FileStream(FileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    long startPos = fs.Position;

                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        CombatEventBase evt = ParseLine(line, out string evtStr);
                        count++;

                        if (evt == null)
                        {
                            if (!string.IsNullOrEmpty(evtStr))
                                unknown.AddValue(evtStr, 1);
                        }
                        else
                        {
                            eventCount.AddValue(evt.EventName, 1);

                            if (_fightMonitorFactory.IsMonitoredFight(evt, state))
                                isInFight = true;
                            else if (isInFight)
                            {
                                var tpl = _fightMonitorFactory.GetFight();
                                var fight = tpl.Item1;
                                var factory = tpl.Item2;

                                factory.StartFight();

                                foreach (var fightEvent in fight.MonitoredFightEvents)
                                {
                                    state.ProcessCombatEvent(fightEvent);
                                    factory.CalculateEvent(fightEvent);
                                }

                                factory.FinalizeFight();

                                foreach (var unmonitoredEvent in fight.NotMonitoredFightEvents)
                                {
                                    state.ProcessCombatEvent(unmonitoredEvent);
                                    _calculatorFactory.CalculateEvent(unmonitoredEvent);
                                }

                                isInFight = false;
                            }
                            else
                            {
                                state.ProcessCombatEvent(evt);
                                _calculatorFactory.CalculateEvent(evt);
                            }
                            
                        }

                        long cur = fs.Position;
                        long total = fs.Length;
                        long deltaCur = cur - startPos;
                        long deltaTotal = total - startPos;

                        ParseCompletionPercent = (float)deltaCur / (float)deltaTotal;
                    }

                }
            }

            IsParsing = false;
            _logger.Log($"``````````````````````````````````````````````````````````````");
            _logger.Log($"Number of unknown events: {unknown.Count}");
            _logger.Log($"--------------------------------------------------------------");
            foreach (var ev in unknown)
                _logger.Log($"{ev.Key}: {ev.Value}");
            _logger.Log($"``````````````````````````````````````````````````````````````");

            _logger.Log($"Number of known events: {eventCount.Count}");
            _logger.Log($"--------------------------------------------------------------");
            foreach (var ev in eventCount)
                _logger.Log($"{ev.Key}: {ev.Value}");
            _logger.Log($"``````````````````````````````````````````````````````````````");

            _calculatorFactory.FinalizeFight();

            return count;
        }


        private CombatEventBase ParseLine(string line, out string evt)
        {
            Regex r = new Regex(@"(\d{1,2})/(\d{1,2})\s(\d{2}):(\d{2}):(\d{2}).(\d{3})\s\s(\w+),(.+)$"); //matches the date format used in the combat log
            Match m = r.Match(line);
            GroupCollection collection = m.Groups;

            if (collection.Count != 9)
            {
                evt = string.Empty;
                return null;
            }

            string month = collection[1].Value;
            string day = collection[2].Value;
            string hour = collection[3].Value;
            string minute = collection[4].Value;
            string second = collection[5].Value;
            string millisecond = collection[6].Value;

            evt = collection[7].Value;
            string data = collection[8].Value;
            string[] dataArray = ParseEventParameters(data);
            DateTime time;

            //This should never error, as the date format is expected to be identical every time
            time = new DateTime(DateTime.Now.Year, int.Parse(month), int.Parse(day), int.Parse(hour), int.Parse(minute), int.Parse(second), int.Parse(millisecond)).ToUniversalTime();

            return _parserFactory.Parse(time, evt, dataArray);
        }


        private string[] ParseEventParameters(string unsplitParameters)
        {
            //Because the combat log can have lines like the following, we need to do a custom parse as opposed to a comma split
            //This custom parse will ignore commas that are inside quotes, and will also remove quotation marks from single values
            //ex "\"Invoke Xuen, the White Tiger\"" becomes "Invoke Xuen, the White Tiger"
            //4/9 07:38:46.299  SPELL_SUMMON,Player-61-07B7D5D6,"Kildonne-Zul'jin",0x511,0x0,Creature-0-3019-1153-26151-73967-000008E9BF,"Xuen",0xa28,0x0,132578,"Invoke Xuen, the White Tiger",0x8

            List<string> dataList = new List<string>();
            int index = 0;
            bool inquote = false;
            int startIndex = 0;
            while (index <= unsplitParameters.Length)
            {
                if (index == unsplitParameters.Length)
                {
                    dataList.Add(unsplitParameters.Substring(startIndex, index - startIndex));
                    break;
                }

                if (unsplitParameters[index] == '"')
                {
                    inquote = !inquote;
                }
                else if (unsplitParameters[index] == ',')
                {
                    if (!inquote)
                    {
                        string s = unsplitParameters.Substring(startIndex, index - startIndex);
                        if (s[0] == '"' && s[s.Length - 1] == '"')
                            s = s.Substring(1, s.Length - 2);
                        dataList.Add(s);
                        startIndex = index + 1;
                    }
                }
                index++;
            }

            return dataList.ToArray();
        }
    }
}
