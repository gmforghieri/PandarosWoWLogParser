using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PandarosWoWLogParser.Models;
using System.Threading.Tasks;
using PandarosWoWLogParser.Parsers;

namespace PandarosWoWLogParser
{
    public class CombatLogParser
    {
        public bool IsParsing { get; private set; } = false;
        public float ParseCompletionPercent { get; private set; } = 0f;
        public FileInfo FileInfo { get; private set; }
        public Queue<CombatEventBase> CombatQueue { get; set; } = new Queue<CombatEventBase>();

        ICombatParser<SpellDamage> _spelldamageParser;
        ICombatParser<SpellPeriodicDamage> _spellPeriodicParser;
        ICombatParser<SwingDamage> _swingDamageParser;
        ICombatParser<SpellBase> _spellParser;
        ICombatParser<SpellFailed> _spellFailedParser;
        ICombatParser<SpellEnergize> _spellEnergize;
        ICombatParser<SpellAura> _spellAura;
        ICombatParser<SpellAuraDose> _spellAuraDose;
        ICombatParser<SpellAuraBrokenSpell> _spellAuraBrokenSpell;
        ICombatParser<SpellMissed> _spellMissedParser;

        public CombatLogParser(ICombatParser<SpellDamage> spelldamageParser, 
                               ICombatParser<SpellPeriodicDamage> spellPeriodicParser,
                               ICombatParser<SpellBase> spellParser,
                               ICombatParser<SpellFailed> spellFailedParser,
                               ICombatParser<SwingDamage> swingDamageParser,
                               ICombatParser<SpellAura> spellAura,
                               ICombatParser<SpellAuraDose> spellAuraDose,
                               ICombatParser<SpellAuraBrokenSpell> spellAuraBrokenSpell,
                               ICombatParser<SpellMissed> spellmissed,
                               ICombatParser<SpellEnergize> spellEnergize)
        {
            _spelldamageParser = spelldamageParser;
            _spellPeriodicParser = spellPeriodicParser;
            _swingDamageParser = swingDamageParser;
            _spellFailedParser = spellFailedParser;
            _spellParser = spellParser;
            _spellEnergize = spellEnergize;
            _spellAura = spellAura;
            _spellAuraDose = spellAuraDose;
            _spellAuraBrokenSpell = spellAuraBrokenSpell;
            _spellMissedParser = spellmissed;
        }

        public void ParseToEnd(string filepath)
        {
            ParseCompletionPercent = 0f;
            IsParsing = true;

            if (File.Exists(filepath))
            {
                FileInfo = new FileInfo(filepath);
            }
            else
                throw new FileNotFoundException("Combat Log not found", filepath);

            using (FileStream fs = new FileStream(FileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    long startPos = fs.Position;

                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        CombatEventBase evt = ParseLine(line);

                        if (evt != null)
                            CombatQueue.Enqueue(evt);

                        long cur = fs.Position;
                        long total = fs.Length;
                        long deltaCur = cur - startPos;
                        long deltaTotal = total - startPos;

                        ParseCompletionPercent = (float)deltaCur / (float)deltaTotal;
                    }

                }
            }
            IsParsing = false;

        }


        private CombatEventBase ParseLine(string line)
        {
            Regex r = new Regex(@"(\d{1,2})/(\d{1,2})\s(\d{2}):(\d{2}):(\d{2}).(\d{3})\s\s(\w+),(.+)$"); //matches the date format used in the combat log
            Match m = r.Match(line);
            GroupCollection collection = m.Groups;

            if (collection.Count != 9)
            {
                Exception e = new Exception("Error parsing line");
                e.Data["line"] = line;
                throw e;
            }

            string month = collection[1].Value;
            string day = collection[2].Value;
            string hour = collection[3].Value;
            string minute = collection[4].Value;
            string second = collection[5].Value;
            string millisecond = collection[6].Value;

            string evt = collection[7].Value;
            string data = collection[8].Value;

            DateTime time;

            //This should never error, as the date format is expected to be identical every time
            time = new DateTime(DateTime.Now.Year, int.Parse(month), int.Parse(day), int.Parse(hour), int.Parse(minute), int.Parse(second), int.Parse(millisecond));

            return ParseObject(evt, data, time);

        }

        private CombatEventBase ParseObject(string evt, string data, DateTime time)
        {
            switch (evt)
            {
                case LogEvents.SWING_DAMAGE:
                    return _swingDamageParser.Parse(time.ToUniversalTime(), evt, ParseEventParameters(data));

                case LogEvents.SPELL_DAMAGE:
                    return _spelldamageParser.Parse(time.ToUniversalTime(), evt, ParseEventParameters(data));

                case LogEvents.SPELL_PERIODIC_DAMAGE:
                    return _spellPeriodicParser.Parse(time.ToUniversalTime(), evt, ParseEventParameters(data));

                case LogEvents.SPELL_CAST_START:
                case LogEvents.SPELL_CAST_SUCCESS:
                    return _spellParser.Parse(time.ToUniversalTime(), evt, ParseEventParameters(data));

                case LogEvents.SPELL_CAST_FAILED:
                    return _spellFailedParser.Parse(time.ToUniversalTime(), evt, ParseEventParameters(data));

                case LogEvents.SPELL_ENERGIZE:
                case LogEvents.SPELL_PERIODIC_ENERGIZE:
                    return _spellEnergize.Parse(time.ToUniversalTime(), evt, ParseEventParameters(data));

                case LogEvents.SPELL_AURA_APPLIED:
                case LogEvents.SPELL_AURA_REMOVED:
                case LogEvents.SPELL_AURA_REFRESH:
                case LogEvents.SPELL_AURA_BROKEN:
                    return _spellAura.Parse(time.ToUniversalTime(), evt, ParseEventParameters(data));

                case LogEvents.SPELL_AURA_APPLIED_DOSE:
                case LogEvents.SPELL_AURA_REMOVED_DOSE:
                    return _spellAuraDose.Parse(time.ToUniversalTime(), evt, ParseEventParameters(data));

                case LogEvents.SPELL_AURA_BROKEN_SPELL:
                    return _spellAuraBrokenSpell.Parse(time.ToUniversalTime(), evt, ParseEventParameters(data));

                case LogEvents.SPELL_MISSED:
                    return _spellMissedParser.Parse(time.ToUniversalTime(), evt, ParseEventParameters(data));

                default:
                    return null;
            }
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
