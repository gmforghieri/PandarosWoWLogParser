namespace PandarosWoWLogParser.Models
{
    public interface ISpellAuraBrokenSpell
    {
        string AuraType { get; set; }
        int ExtraSpellID { get; set; }
        string ExtraSpellName { get; set; }
        SpellSchool ExtraSpellSchool { get; set; }
    }
}