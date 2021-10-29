using PandarosWoWLogParser.Calculators;
using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PandarosWoWLogParser.FightMonitor
{
    public class FightMonitorFactory : IFightMonitorFactory
    {
        public static List<string> CombatEventsTriggerInFight { get; set; } = new List<string>()
        {
            LogEvents.SPELL_DAMAGE,
            LogEvents.SPELL_PERIODIC_DAMAGE,
            LogEvents.RANGE_MISSED,
            LogEvents.RANGE_DAMAGE,
            LogEvents.SPELL_MISSED,
            LogEvents.SWING_DAMAGE,
            LogEvents.SWING_MISSED,
            LogEvents.UNIT_DIED
        };

        IPandaLogger _logger;
        IStatsReporter _reporter;

        public FightMonitorFactory(IPandaLogger logger, IStatsReporter reporter)
        {
            _reporter = reporter;
            _logger = logger;
        }

        public bool IsMonitoredFight(ICombatEvent evnt, ICombatState state)
        {
            if (!state.InFight && CombatEventsTriggerInFight.Contains(evnt.EventName))
            {
                string npcName = string.Empty;
                string npcId = string.Empty;

                if (evnt.SourceFlags.GetController == UnitFlags.Controller.Npc)
                {
                    npcName = evnt.SourceName;
                    npcId = evnt.SourceGuid;
                }
                else if (evnt.DestFlags.GetController == UnitFlags.Controller.Npc)
                {
                    npcName = evnt.DestName;
                    npcId = evnt.DestGuid;
                }
                
                if (!string.IsNullOrEmpty(npcName))
                {
                    state.InFight = true;
                    state.CurrentFight = new MonitoredFight()
                    {
                        BossName = npcName,
                        FightStart = evnt.Timestamp,
                        MonsterID = new Dictionary<string, bool>() { { npcId, false } }
                    };
                    state.CalculatorFactory = new CalculatorFactory(_logger, _reporter, state, state.CurrentFight);
                }
            }

            if (state.InFight)
            {
                state.InFight = state.CurrentFight.AddEvent(evnt, state);
            }

            return state.InFight;
        }
    }
}
