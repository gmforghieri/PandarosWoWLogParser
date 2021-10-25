# PandarosWoWLogParser

WoW 3.3.5 log parser code.

Uses DI via Autofac or Service builder.

Example:

```
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PandarosWoWLogParser;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Text;

namespace PandarosWoWLogParser.Tests
{
    [TestClass()]
    public class PandarosParserTests
    {
        [TestMethod()]
        public void PandarosParserSetupTest()
        {
            var builder = new ContainerBuilder();
            var logger = new PandaLogger("C:/temp/");
            builder.PandarosParserSetup(logger, logger);

            var Container = builder.Build();

            var clp = Container.Resolve<CombatLogParser>();

            logger.Log("Starting Parse.");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            clp.ParseToEnd("C:/Program Files/Ascension Launcher/resources/client/Logs/WoWCombatLog.log");
            sw.Stop();
            logger.Log($"Parsed in {sw.Elapsed}.");
        }
    }
}
```

Stats output to log file.

Update MonitoredZones.json to add more fights to calculate.