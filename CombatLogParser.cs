using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PandarosWoWLogParser.Models;
using System.Threading.Tasks;
using PandarosWoWLogParser.Parsers;
using PandarosWoWLogParser.Calculators;
using PandarosWoWLogParser.FightMonitor;

namespace PandarosWoWLogParser
{
    public class CombatLogParser
    {
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

        public void ParseToEnd(string filepath)
        {
            FileInfo fileToParse;

            if (File.Exists(filepath))
            {
                fileToParse = new FileInfo(filepath);
            }
            else
                throw new FileNotFoundException("Combat Log not found", filepath);


            CombatState state = new CombatState(_parserFactory, _fightMonitorFactory, _logger, _reporter);

            using (FileStream fs = new FileStream(fileToParse.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    long startPos = fs.Position;

                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        CombatEventBase evt = ParseLine(line, out string evtStr);

                        state.ProcessCombatEvent(evt, evtStr);
                    }

                }
            }

            state.ParseComplete();
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
            time = new DateTime(DateTime.Now.Year, int.Parse(month), int.Parse(day), int.Parse(hour), int.Parse(minute), int.Parse(second), int.Parse(millisecond));

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
