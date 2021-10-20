using PandarosWoWLogParser.FightMonitor;
using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PandarosWoWLogParser.Calculators
{
    public class DamageDoneBySpellCalculator : BaseCalculator
    {
        Dictionary<string, Dictionary<string, long>> _damageSpellByPlayer = new Dictionary<string, Dictionary<string, long>>();

        public DamageDoneBySpellCalculator(IPandaLogger logger, IStatsReporter reporter, CombatState state, MonitoredFight fight) : base(logger, reporter, state, fight)
        {
            ApplicableEvents = new List<string>()
            {
                LogEvents.SPELL_DAMAGE,
                LogEvents.RANGE_DAMAGE,
                LogEvents.SWING_DAMAGE,
                LogEvents.SPELL_PERIODIC_DAMAGE
            };
        }

        public override void CalculateEvent(ICombatEvent combatEvent)
        {
            var damage = (IDamage)combatEvent;

            if (combatEvent.SourceFlags.GetFlagType == UnitFlags.FlagType.Player)
                AddDamage(combatEvent.SourceName, combatEvent, damage);
            else if (State.TryGetOwnerName(combatEvent, out var owner))
            {
                AddDamage(owner, combatEvent, damage);
            }
        }

        private void AddDamage(string owner, ICombatEvent combatEvent, IDamage damage)
        {
            switch (combatEvent.EventName)
            {
                case LogEvents.SWING_DAMAGE:
                    _damageSpellByPlayer.AddValue(owner, "Auto attack", damage.Damage);
                    break;

                case LogEvents.RANGE_DAMAGE:
                    _damageSpellByPlayer.AddValue(owner, "Shoot", damage.Damage);
                    break;

                case LogEvents.SPELL_DAMAGE:
                case LogEvents.SPELL_PERIODIC_DAMAGE:
                    var spell = (ISpell)combatEvent;
                    _damageSpellByPlayer.AddValue(owner, spell.SpellName, damage.Damage);
                    break;
            }
        }

        public override void FinalizeFight()
        {
            _statsReporting.Report(_damageSpellByPlayer, "Damage Rankings", Fight, State);
        }

        public override void StartFight()
        {
            
        }
    }
}
