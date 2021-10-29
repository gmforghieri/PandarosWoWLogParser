using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PandarosWoWLogParser.Calculators
{
    public class PlayerDiedCalculator : BaseCalculator
    {
        Dictionary<string, long> _playerDeaths = new Dictionary<string, long>();

        public PlayerDiedCalculator(IPandaLogger logger, IStatsReporter reporter, ICombatState state, MonitoredFight fight) : base(logger, reporter, state, fight)
        {
            ApplicableEvents = new List<string>()
        {
            LogEvents.UNIT_DIED
        };
        }

        public override void CalculateEvent(ICombatEvent combatEvent)
        {
            if (combatEvent.DestFlags.GetFlagType == UnitFlags.FlagType.Player)
            {
                _playerDeaths.AddValue(combatEvent.DestName, 1);
            }
        }

        public override void FinalizeFight()
        {
            _statsReporting.Report(_playerDeaths, "Player Deaths", Fight, State);
        }

        public override void StartFight()
        {

        }
    }
}
