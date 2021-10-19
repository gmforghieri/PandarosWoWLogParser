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
using System.Configuration;
using Newtonsoft.Json;
using System.IO;

namespace PandarosWoWLogParser
{
    class Program
    {
        public static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            var logger = new PandaLogger(ConfigurationManager.AppSettings.Get("outputDir"));
            builder.RegisterInstance(logger).As<IPandaLogger>().SingleInstance();
            builder.RegisterInstance(logger).As<IStatsReporter>().SingleInstance();
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

            var monitoredZones = JsonConvert.DeserializeObject<List<MonitoredZone>>(File.ReadAllText("./MonitoredZones.json"));
            builder.RegisterInstance(monitoredZones);
            builder.RegisterType<FightMonitorFactory>().As<IFightMonitorFactory>().SingleInstance();
            builder.RegisterType<CombatLogParser>();

            Container = builder.Build();

            var clp = Container.Resolve<CombatLogParser>();

            logger.Log("Starting Parse.");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var count = clp.ParseToEnd(ConfigurationManager.AppSettings.Get("logfile"));
            sw.Stop();
            logger.Log($"Parsed {count} events in {sw.Elapsed}.");
            Thread.Sleep(1000);
        }
    }
}
