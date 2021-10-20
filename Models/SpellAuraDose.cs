using CombatLogParser;

namespace PandarosWoWLogParser.Models
{
    public class SpellAuraDose : SpellBase, ISpellAuraDose, ISpellAura
    {
        public BuffType AuraType { get; set; }
        public int AuraDoeseAdded { get; set; }
    }
}
