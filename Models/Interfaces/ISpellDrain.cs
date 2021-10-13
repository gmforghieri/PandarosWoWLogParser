namespace PandarosWoWLogParser.Models
{
    public interface ISpellDrain
    {
        int DrainAmount { get; set; }
        int ExtraDrainAmount { get; set; }
        PowerType PowerType { get; set; }
    }
}