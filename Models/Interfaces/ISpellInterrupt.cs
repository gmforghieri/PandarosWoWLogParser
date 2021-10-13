namespace PandarosWoWLogParser.Models
{
    public interface ISpellInterrupt
    {
        SpellSchool ExtraSchool { get; set; }
        int ExtraSpellId { get; set; }
        string ExtraSpellName { get; set; }
    }
}