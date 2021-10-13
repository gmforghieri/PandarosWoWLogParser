namespace PandarosWoWLogParser.Models
{
    public interface ISpellMissed
    {
        int MissDamage { get; set; }
        string MissType { get; set; }
    }
}