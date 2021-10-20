using CombatLogParser;

namespace PandarosWoWLogParser.Models
{
    public interface ISpellAura
    {
        BuffType AuraType { get; set; }
    }
}