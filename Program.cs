using PandarosWoWLogParser.Models;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using PandarosWoWLogParser.Parsers;
using System.Collections.Generic;
using PandarosWoWLogParser.Calculators;
using PandarosWoWLogParser.FightMonitor;

namespace PandarosWoWLogParser
{
    class Program
    {
        public static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<SpellDamageParser>().As<ICombatParser<SpellDamage>>().SingleInstance();
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
            builder.RegisterType<SpellInterruptParser>().As<ICombatParser<SpellInterrupt>>().SingleInstance();
            builder.RegisterType<SpellDispelParser>().As<ICombatParser<SpellDispel>>().SingleInstance();
            builder.RegisterType<SpellDrainParser>().As<ICombatParser<SpellDrain>>().SingleInstance();
            builder.RegisterType<EnchantParser>().As<ICombatParser<Enchant>>().SingleInstance();
            builder.RegisterType<ParserFactory>().As<IParserFactory>().SingleInstance();

            List<MonitoredZone> monitoredZones = new List<MonitoredZone>()
            {
                new MonitoredZone()
                {
                    ZoneName = "Test",
                    MonitoredFights = new List<string>()
                    {
                        "Watchkeeper Gargolmar"
                    }
                }
            };

            builder.RegisterInstance(monitoredZones);
            builder.RegisterType<FightMonitorFactory>().As<IFightMonitorFactory>().SingleInstance();


            List<ICalculator> calculators = new List<ICalculator>()
            {
                new TotalDamageCalculator()
            };


            builder.RegisterInstance(calculators);
            builder.RegisterType<CalculatorFactory>().As<ICalculatorFactory>().SingleInstance();
            builder.RegisterType<CombatLogParser>();

            Container = builder.Build();

            var clp = Container.Resolve<CombatLogParser>();

            Console.WriteLine("Starting Parse.");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var count = clp.ParseToEnd(@"C:\Program Files\Ascension Launcher\resources\client\Logs\WoWCombatLog.log");
            sw.Stop();
            Console.WriteLine($"Parsed {count} events in {sw.Elapsed}.");

        }
    }
}
