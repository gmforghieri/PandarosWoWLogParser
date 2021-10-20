namespace PandarosWoWLogParser.Models
{
    public interface ISpellAuraDose : ISpellAura
    {
        int AuraDoeseAdded { get; set; }
    }
}