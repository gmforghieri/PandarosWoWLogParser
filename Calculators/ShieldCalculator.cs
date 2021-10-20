using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Calculators
{
    public class ShieldCalculator : BaseCalculator
    {
        Dictionary<string, Dictionary<string, long>> _shieldGivenDoneByPlayersTotal = new Dictionary<string, Dictionary<string, long>>();

        private List<string> _shieldNames = new List<string>()
        {
            "Ardent Defender",
            "Divine Aegis",
            "Mana Sheld",
            "Fire Ward",
            "Frost Ward",
            "Sacred Shield",
            "Stoneclaw Totem",
            "Shadow Ward",
            "Ice Barrier",
            "Unfair Advantage",
            "Into Darkness",
            "Power Word: Shield",
        };

        public ShieldCalculator(IPandaLogger logger, IStatsReporter reporter, CombatState state, MonitoredFight fight) : base(logger, reporter, state, fight)
        {
            ApplicableEvents = new List<string>()
            {
                LogEvents.SPELL_DAMAGE,
                LogEvents.RANGE_DAMAGE,
                LogEvents.SWING_DAMAGE,
                LogEvents.SPELL_PERIODIC_DAMAGE,
                LogEvents.DAMAGE_SHIELD
            };
        }

        public override void CalculateEvent(ICombatEvent combatEvent)
        {
            var damage = (IDamage)combatEvent;

            if (combatEvent.SourceFlags.GetFlagType == UnitFlags.FlagType.Npc && 
                combatEvent.DestFlags.GetFlagType == UnitFlags.FlagType.Player &&
                damage.Absorbed > 0 && 
                State.PlayerBuffs.TryGetValue(combatEvent.DestName, out var buffs))
            {
                Dictionary<string, string> sheilds = new Dictionary<string, string>();
                foreach (var shield in _shieldNames)
                {
                    if (buffs.TryGetValue(shield, out var caster))
                    {
                        sheilds[shield] = caster;
                    }
                }

                if (sheilds.Count != 0)
                {
                    float dmg = damage.Absorbed / sheilds.Count;

                    foreach (var s in sheilds)
                        _shieldGivenDoneByPlayersTotal.AddValue(s.Value, s.Key, Convert.ToInt32(Math.Round(dmg)));
                }
            }
        }

        public override void FinalizeFight()
        {
            _statsReporting.Report(_shieldGivenDoneByPlayersTotal, "Damage Prevented with Shields (absorb) by caster", Fight, State);
        }

        public override void StartFight()
        {

        }
    }
}
