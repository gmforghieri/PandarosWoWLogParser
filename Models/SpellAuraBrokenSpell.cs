using CombatLogParser;

namespace PandarosWoWLogParser.Models
{
    public class SpellAuraBrokenSpell : SpellBase, ISpellAuraBrokenSpell, ISpellAura
    {
        public int ExtraSpellID { get; set; }
        public string ExtraSpellName { get; set; }
        public SpellSchool ExtraSpellSchool { get; set; }
        public BuffType AuraType { get; set; }
    }
}
