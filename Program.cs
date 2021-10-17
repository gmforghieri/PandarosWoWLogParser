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
            var logger = new PandaLogger("C:/temp/");
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

            List<MonitoredZone> monitoredZones = new List<MonitoredZone>()
            {
                new MonitoredZone()
                {
                    ZoneName = "Serpentshrine Cavern",
                    MonitoredFights = new Dictionary<string, List<string>>()
                    {
                        { "Lady Vashj", new List<string>() { "Lady Vashj", "Tainted Elementals", "Toxic Sporebat", "Coilfang Strider", "Coilfang Elite", "Enchanted Elemental" } },
                        { "Hydross the Unstable", new List<string>() { "Hydross the Unstable" } },
                        { "The Lurker Below", new List<string>() { "The Lurker Below" } },
                        { "Leotheras the Blind", new List<string>() { "Leotheras the Blind" } },
                        { "Fathom-Lord Karathress", new List<string>() { "Fathom-Guard Sharkkis", "Fathom-Guard Tidalvess", "Fathom-Guard Caribdis" } },
                        { "Morogrim Tidewalker", new List<string>() { "Morogrim Tidewalker" } }
                    }
                },
                new MonitoredZone()
                {
                    ZoneName = "Karazhan",
                    MonitoredFights = new Dictionary<string, List<string>>()
                    {
                        { "Attumen the Huntsman", new List<string>() { "Attumen the Huntsman" } },
                        { "Moroes", new List<string>() { "Moroes", "Lady Catriona Von'Indi", "Lady Keira Berrybuck", "Baroness Dorothea Millstipe", "Baron Rafe Dreuger", "Lord Robin Daris, Moroes", "Lord Crispin Ference" } },
                        { "Maiden of Virtue", new List<string>() { "Maiden of Virtue" } },
                        { "Opera House", new List<string>() { "Dorothee", "Tito", "Tinhead", "Strawman", "Roar", "Crone", "Big Bad Wolf", "Romulo", "Julianne" } },
                        { "The Curator", new List<string>() { "The Curator" } },
                        { "Terestian Illhoof", new List<string>() { "Terestian Illhoof", "Kil'rek" } },
                        { "Shade of Aran", new List<string>() { "Shade of Aran" } },
                        { "Netherspite", new List<string>() { "Netherspite" } },
                        { "Prince Malchezaar", new List<string>() { "Prince Malchezaar" } },
                        { "Nightbane", new List<string>() { "Nightbane" } },
                        { "Rokad the Ravager", new List<string>() { "Rokad the Ravager" } },
                        { "Shadikith the Glider", new List<string>() { "Shadikith the Glider" } },
                        { "Hyakiss the Lurker", new List<string>() { "Hyakiss the Lurker" } }
                    }
                },
                new MonitoredZone()
                {
                    ZoneName = "Shattered Halls",
                    MonitoredFights = new Dictionary<string, List<string>>()
                    {
                        { "Warchief Kargath Bladefist", new List<string>() { "Warchief Kargath Bladefist" } }
                    }
                }
            };

            builder.RegisterInstance(monitoredZones);
            builder.RegisterType<FightMonitorFactory>().As<IFightMonitorFactory>().SingleInstance();
            builder.RegisterType<CombatLogParser>();

            Container = builder.Build();

            var clp = Container.Resolve<CombatLogParser>();

            logger.Log("Starting Parse.");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var count = clp.ParseToEnd(@"C:\Program Files\Ascension Launcher\resources\client\Logs\WoWCombatLog.log");
            sw.Stop();
            logger.Log($"Parsed {count} events in {sw.Elapsed}.");
            Thread.Sleep(1000);
        }
    }
}
