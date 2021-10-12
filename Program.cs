using PandarosWoWLogParser.Models;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using PandarosWoWLogParser.Parsers;

namespace PandarosWoWLogParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<SpellDamageParser>().As<ICombatParser<SpellDamage>>().SingleInstance();
            builder.RegisterType<SpellPeriodicDamageParser>().As<ICombatParser<SpellPeriodicDamage>>().SingleInstance();
            builder.RegisterType<SwingDamageParser>().As<ICombatParser<SwingDamage>>().SingleInstance();
            builder.RegisterType<CombatLogParser>();

            var container = builder.Build();

            var clp = container.Resolve<CombatLogParser>();
            Console.WriteLine("Starting Parse.");
            clp.ParseToEnd(@"C:\Program Files\Ascension Launcher\resources\client\Logs\WoWCombatLog.log");

            while (clp.CombatQueue.TryDequeue(out var obj))
            {
                if (obj.EventName == Events.SPELL_DAMAGE)
                {
                    var spell = obj as SpellDamage;
                    Console.WriteLine(obj.EventName + ": source- " + obj.SourceName + " dest- " + obj.DestName + " damage- " + spell.Damage + " school- " + spell.DamageSchool.ToString());
                }
                
            }
        }
    }
}
