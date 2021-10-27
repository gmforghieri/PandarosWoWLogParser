using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PandarosWoWLogParser.Calculators
{
    public class CalculatorFactory : ICalculatorFactory
    {
        public Dictionary<string, List<ICalculator>> Calculators { get; set; } = new Dictionary<string, List<ICalculator>>();
        public List<ICalculator> CalculatorFlatList { get; set; } = new List<ICalculator>();
        public ICombatState State { get; set; }
        public MonitoredFight Fight { get; set; }
        Dictionary<string, int> _eventCount = new Dictionary<string, int>();
        IPandaLogger _logger;
        IStatsReporter _reporter;

        public CalculatorFactory(IPandaLogger logger, IStatsReporter reporter, ICombatState state, MonitoredFight fight)
        {
            var assem = Assembly.GetExecutingAssembly();
            _logger = logger;
            _reporter = reporter;
            State = state;
            Fight = fight;
            var typeArray = assem.GetTypes();

            foreach (var type in typeArray)
            {
                if (type.GetInterfaces().Any(i => i == typeof(ICalculator)) && !type.IsAbstract)
                    CalculatorFlatList.Add(Activator.CreateInstance(type, logger, _reporter, state, fight) as ICalculator);
            }

            foreach (var calc in CalculatorFlatList)
                foreach(var evnt in calc.ApplicableEvents)
                {
                    if (Calculators.TryGetValue(evnt, out var list))
                        list.Add(calc);
                    else
                        Calculators.Add(evnt, new List<ICalculator>() { calc });
                }
        }

        public void CalculateEvent(ICombatEvent combatEvent)
        {
            if (State.InFight)
            {
                if (!_eventCount.TryGetValue(combatEvent.EventName, out int val))
                    _eventCount[combatEvent.EventName] = 1;
                else
                    _eventCount[combatEvent.EventName] = val + 1;
            }

            if (Calculators.TryGetValue(combatEvent.EventName, out var calcList))
                foreach (var calc in calcList)
                    calc.CalculateEvent(combatEvent);
           }


        public void StartFight()
        {
            _eventCount.Clear();
            _logger.Log("---------------------------------------------");
            _logger.Log($"{Fight.FightStart.ToLocalTime()} Fight Start: {Fight.CurrentZone.ZoneName} - {Fight.BossName}");
            _logger.Log("---------------------------------------------");
            foreach (var calc in CalculatorFlatList)
                calc.StartFight();
        }

        public void FinalizeFight()
        {
            foreach (var calc in CalculatorFlatList)
                calc.FinalizeFight();

            _logger.Log("---------------------------------------------");
            _logger.Log($"{Fight.FightEnd.ToLocalTime()} Fight End: {Fight.CurrentZone.ZoneName} - {Fight.BossName} ({Fight.FightEnd.Subtract(Fight.FightStart)})");
            foreach (var ev in _eventCount)
                _logger.Log($"{ev.Key}: {ev.Value}");
            _logger.Log("---------------------------------------------");
        }
    }
}
