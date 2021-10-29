using PandarosWoWLogParser.Calculators;
using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;
using PandarosWoWLogParser.Parsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser
{
    public class CombatState : CombatStateBase
    {
        public CombatState(IFightMonitorFactory fightMonitorFactory, IPandaLogger logger) : base(fightMonitorFactory, logger)
        {
            
        }

        public override void ProcessCombatEvent(ICombatEvent combatEvent, string evtStr)
        {
            base.ProcessCombatEvent(combatEvent, evtStr);

            if (combatEvent != null)
            {
                if (_fightMonitorFactory.IsMonitoredFight(combatEvent, this))
                    _prevFightState = true;
                else if (_prevFightState)
                {
                    CalculatorFactory.StartFight();

                    foreach (var fightEvent in CurrentFight.MonitoredFightEvents)
                    {
                        ProcessCombatEventInternal(fightEvent);
                        CalculatorFactory.CalculateEvent(fightEvent);
                    }

                    CalculatorFactory.FinalizeFight();

                    _prevFightState = false;
                    CalculatorFactory = null;
                    CurrentFight = null;
                }

            }
        }
    }
}
