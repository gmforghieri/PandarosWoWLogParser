using PandarosWoWLogParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser.Parsers
{
    public class EnchantParser : BaseParser, ICombatParser<Enchant>
    {
        public new Enchant Parse(DateTime timestamp, string eventName, string[] eventData)
        {
            return Parse(timestamp, eventName, eventData, new Enchant());
        }

        public Enchant Parse(DateTime timestamp, string eventName, string[] eventData, Enchant obj)
        {
            obj = (Enchant)base.Parse(timestamp, eventName, eventData, obj);
            obj.SpellName = eventData[Indexes.ENCHANT.SpellName];
            obj.ItemID = eventData[Indexes.ENCHANT.ItemID].ToInt();
            obj.ItemName = eventData[Indexes.ENCHANT.ItemName];
            return obj;
        }
    }
}
