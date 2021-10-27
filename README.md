# PandarosWoWLogParser

WoW 3.3.5 log parser code.

Uses DI via Autofac or Service builder.

Example to parse:

```c#
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

Combine log files in a directory. Outputs to the directory in a .log file.

When combining log files you can put the timezone of the log file in [] brackets in the file name to have the correct localization when cobining. Ex: WoWCombatLog[Eastern Standard Time].txt or WoWCombatLog[Mountain Standard Time].txt

Example to combine log files:

```c#
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PandarosWoWLogParser;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace PandarosWoWLogParser.Tests
{
    [TestClass()]
    public class PandarosLogCombinerTests
    {
        [TestMethod()]
        public void PandarosCombinerSetupTest()
        {
            var builder = new ContainerBuilder();
            var logger = new PandaLogger("C:/temp/");
            builder.PandarosParserSetup(logger, logger);

            var Container = builder.Build();

            var clp = Container.Resolve<CombatLogCombiner>();

            logger.Log("Starting Parse.");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            clp.ParseToEnd(@"C:\Program Files\Ascension Launcher\resources\client\Logs\1025\");
            sw.Stop();
            logger.Log($"Parsed in {sw.Elapsed}.");
            Thread.Sleep(1000);
        }
    }
}
```