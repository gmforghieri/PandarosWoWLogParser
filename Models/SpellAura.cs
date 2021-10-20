using CombatLogParser;

namespace PandarosWoWLogParser.Models
{
    public class SpellAura : SpellBase, ISpellAura
    {
        public BuffType AuraType { get; set; }
    }
}
