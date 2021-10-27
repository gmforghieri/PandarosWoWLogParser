using PandarosWoWLogParser.Calculators;
using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;
using PandarosWoWLogParser.Parsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser
{
    public class AllCombatsState : CombatStateBase
    {
        MonitoredFight _allFights = new MonitoredFight()
        {
            CurrentZone = new MonitoredZone()
            {
                ZoneName = "All",
                MonitoredFights = new Dictionary<string, List<string>>()
            },
            BossName = "All Fights in Log"
        };

        public AllCombatsState(IFightMonitorFactory fightMonitorFactory, IPandaLogger logger, IStatsReporter reporter) : base(fightMonitorFactory, logger)
        {
            CurrentFight = _allFights;
            CalculatorFactory = new CalculatorFactory(logger, reporter, this, _allFights);
            CalculatorFactory.StartFight();
        }

        public override void ProcessCombatEvent(ICombatEvent combatEvent, string evtStr)
        {
            base.ProcessCombatEvent(combatEvent, evtStr);

            if (combatEvent != null)
            {
                ProcessCombatEventInternal(combatEvent);
                CalculatorFactory.CalculateEvent(combatEvent);
            }
        }

        public override void ParseComplete()
        {
            CalculatorFactory.FinalizeFight();
            base.ParseComplete();
        }
    }
}
