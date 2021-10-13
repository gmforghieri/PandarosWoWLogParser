namespace PandarosWoWLogParser.Models
{
    public interface ISpellBase
    {
        SpellSchool School { get; set; }
        int SpellId { get; set; }
        string SpellName { get; set; }
    }
}