using PandarosWoWLogParser.Models;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using PandarosWoWLogParser.Parsers;
using System.Collections.Generic;

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
            builder.RegisterType<SpellFailedParser>().As<ICombatParser<SpellFailed>>().SingleInstance();
            builder.RegisterType<SpellParser>().As<ICombatParser<SpellBase>>().SingleInstance();
            builder.RegisterType<SpellEnergizeParser>().As<ICombatParser<SpellEnergize>>().SingleInstance();
            builder.RegisterType<SpellAuraParser>().As<ICombatParser<SpellAura>>().SingleInstance();
            builder.RegisterType<SpellAuraDoseParser>().As<ICombatParser<SpellAuraDose>>().SingleInstance();
            builder.RegisterType<SpellAuraBrokenSpellParser>().As<ICombatParser<SpellAuraBrokenSpell>>().SingleInstance();
            builder.RegisterType<SpellMissedParser>().As<ICombatParser<SpellMissed>>().SingleInstance();
            builder.RegisterType<SwingMissedParser>().As<ICombatParser<SwingMissed>>().SingleInstance();
            builder.RegisterType<SwingMissedParser>().As<ICombatParser<SwingMissed>>().SingleInstance();
            builder.RegisterType<SpellHealParser>().As<ICombatParser<SpellHeal>>().SingleInstance();
            builder.RegisterType<BaseParser>().As<ICombatParser<CombatEventBase>>().SingleInstance();
            builder.RegisterType<EnviormentalDamageParser>().As<ICombatParser<EnviormentalDamage>>().SingleInstance();
            builder.RegisterType<CombatLogParser>();

            var container = builder.Build();

            var clp = container.Resolve<CombatLogParser>();

            Console.WriteLine("Starting Parse.");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            clp.ParseToEnd(@"C:\Program Files\Ascension Launcher\resources\client\Logs\WoWCombatLog.log");
            sw.Stop();
            Console.WriteLine($"Parsed {clp.CombatQueue.Count} events in {sw.Elapsed}.");
            Dictionary<string, int> eventCount = new Dictionary<string, int>();

            while (clp.CombatQueue.TryDequeue(out var obj))
            {
                if (!eventCount.TryGetValue(obj.EventName, out int val))
                    eventCount[obj.EventName] = 1;
                else
                    eventCount[obj.EventName] = val + 1;
            }

            foreach (var ev in eventCount)
                Console.WriteLine($"{ev.Key}: {ev.Value}");
        }
    }
}
