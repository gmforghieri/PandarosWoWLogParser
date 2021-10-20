using PandarosWoWLogParser.FightMonitor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace PandarosWoWLogParser
{
    /// <summary>
    /// Actions that apply to system files.
    /// </summary>
    public enum FileAction : int
    {
        /// <summary>
        /// A command to move a file.
        /// </summary>
        Move = 0,
        /// <summary>
        /// A command to delete a file.
        /// </summary>
        Delete = 1
    }

    /// <summary>
    /// Look for file in the format xxxx.99999.log
    /// The number is extracted from both filenames and this number is used for the comparison
    /// </summary>
    public class LogFileSorter : Comparer<string>
    {
        public override int Compare(string x, string y)
        {

            if (string.IsNullOrEmpty(x))
            {
                if (string.IsNullOrEmpty(y))
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(y))
                {
                    return 1;
                }
                else
                {
                    int xval = PandaLogger.GetNumFromFilename(x);
                    int yval = PandaLogger.GetNumFromFilename(y);
                    return ((new CaseInsensitiveComparer()).Compare(xval, yval));
                }
            }
        }
    }

    public class PandaLogger : IPandaLogger
    {
        public readonly string LOG_DIR;
        string LOG_NAME;
        const string ONE_DOT_LOG = ".1.log";
        const string DOT_STAR_DOT_LOG = ".*.log";
        const string DOT_LOG = ".log";
        static readonly char[] dot = new char[] { '.' };
        const int LOGGER_TRY = 1000;
        Thread _thread;
        Queue<string> _logQueue = new Queue<string>();
        AutoResetEvent _loggerSemaphore = new AutoResetEvent(false);
        string _logFile;

        public PandaLogger(string logDir)
        {
            LOG_DIR = logDir;

            if (logDir.Last() != '/' || LOG_DIR.Last() != '\\')
                LOG_DIR += '/';

            if (!Directory.Exists(LOG_DIR))
                Directory.CreateDirectory(LOG_DIR);

            LOG_NAME = "PandarosLogParser." + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");

            _logFile = LOG_DIR + LOG_NAME + DOT_LOG;
            _thread = new Thread(new ThreadStart(Log));
            _thread.IsBackground = true;
            _thread.Start();
        }

        public void Log(string message, params object[] args)
        {
            if (args != null && args.Length != 0)
                GetFormattedMessage(string.Format(message, args));
            else
                GetFormattedMessage(message);
        }

        public void LogError(Exception e, string message)
        {
            LogError(e);

            if (e.InnerException != null)
                LogError(e.InnerException);
        }

        public void LogError(Exception e, string message, params object[] args)
        {
            if (e.InnerException != null)
                LogError(e.InnerException);
        }

        public void LogError(Exception e)
        {
            lock (_logQueue)
            {
                _logQueue.Enqueue(e.Message);
                _logQueue.Enqueue(e.StackTrace);
            }
            _loggerSemaphore.Set();

            if (e.InnerException != null)
                LogError(e.InnerException);
        }

        private string GetFormattedMessage(string message)
        {
            message = string.Format("[{0}] {1}", DateTime.Now, message);

            lock (_logQueue)
                _logQueue.Enqueue(message);
            _loggerSemaphore.Set();
            return message;
        }

        private void Log()
        {
            while (true)
            {
                _loggerSemaphore.WaitOne(2000);

                using (var sw = new StreamWriter(_logFile, true))
                    while (_logQueue.Count != 0)
                    {
                        var queuedMessage = string.Empty;

                        lock (_logQueue)
                            queuedMessage = _logQueue.Dequeue();

                        if (!string.IsNullOrEmpty(queuedMessage))
                        {
                            try
                            {
                                Console.WriteLine(queuedMessage);
                                sw.WriteLine(queuedMessage);
                            }
                            finally
                            {

                            }
                        }
                    }

                RotateLogs();
            }
        }

        private void RotateLogs()
        {
            if (!File.Exists(_logFile)) return;

            foreach (var fi in new DirectoryInfo(LOG_DIR).GetFiles().OrderByDescending(x => x.LastWriteTime).Skip(20))
                fi.Delete();

            FileInfo fix = new FileInfo(_logFile);
            long fixedSize = fix.Length / 1024L;
            if (fixedSize >= 1024)
            {
                string oldFilename = LOG_DIR + LOG_NAME + ONE_DOT_LOG;
                if (File.Exists(oldFilename))
                {
                    RotateOld(LOG_DIR);
                }
                int j = 0;
                while (!LoggerCheck(_logFile, oldFilename, FileAction.Move))
                {
                    j++;
                    if (j > LOGGER_TRY) break;
                }
            }
        }

        private void RotateOld(string logFileDir)
        {
            string[] raw = Directory.GetFiles(logFileDir, LOG_NAME + DOT_STAR_DOT_LOG);
            ArrayList files = new ArrayList();
            files.AddRange(raw);

            Comparer<string> myComparer = new LogFileSorter();
            files.Sort(myComparer);
            files.Reverse();

            foreach (string f in files)
            {
                int logfnum = GetNumFromFilename(f);
                if (logfnum > 0)
                {
                    string newname = string.Format("{0}{1}.{2}.log", LOG_DIR, LOG_NAME, logfnum + 1);

                    if (logfnum >= 5)
                    {
                        int j = 0;
                        while (!LoggerCheck(f, string.Empty, FileAction.Delete))
                        {
                            j++;
                            if (j > LOGGER_TRY) break;
                        }
                    }
                    else
                    {
                        int j = 0;
                        while (!LoggerCheck(f, newname, FileAction.Move))
                        {
                            j++;
                            if (j > LOGGER_TRY) break;
                        }
                    }
                }
            }
        }

        private bool LoggerCheck(string f, string oldFilename, FileAction action)
        {
            if (File.Exists(f))
            {
                if (action == FileAction.Move)
                {
                    try
                    {
                        if (File.Exists(oldFilename))
                            File.Delete(oldFilename);
                        File.Move(f, oldFilename);
                        return true;
                    }
                    catch { }

                }
                else if (action == FileAction.Delete)
                {
                    try
                    {
                        File.Delete(f);
                        return true;
                    }
                    catch { }
                }
            }
            return false;
        }

        /// <summary>
        /// Return the log number for a file in the format xxxx.9999.log
        /// If there is an error return 0
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static int GetNumFromFilename(string filename)
        {
            int filenum = 0;
            if (!string.IsNullOrEmpty(filename))
            {
                string[] xtokens = filename.Split(dot);
                int xtokencount = xtokens.GetLength(0);
                if (xtokencount > 2)
                {
                    int xval = 0;
                    if (Int32.TryParse(xtokens[xtokencount - 2], out xval))
                    {
                        filenum = xval;
                    }
                }
            }
            return filenum;
        }

        public void ReportPerSecondNumbers<T>(Dictionary<T, long> stats, string name, MonitoredFight fight, CombatState state)
        {
            int i = 0;
            var ts = fight.FightEnd.Subtract(fight.FightStart);

            if (ts.TotalSeconds <= 0)
                return;

            Log("---------------------------------------------");
            Log($"{name}: {fight.CurrentZone.ZoneName} - {fight.BossName}");
            Log("---------------------------------------------");
            foreach (var kvp in stats.OrderBy(i => i.Value).Reverse())
            {
                i++;
                Log($"{i}. {kvp.Key}: {(kvp.Value / ts.TotalSeconds).ToString("N")}");
            }
            Log("---------------------------------------------");
        }

        public void Report<T>(Dictionary<T, long> stats, string name, MonitoredFight fight, CombatState state)
        {
            int i = 0;
            var ts = fight.FightEnd.Subtract(fight.FightStart);
            var total = stats.Sum(kvp => kvp.Value);
            Log("---------------------------------------------");
            Log($"{name}: {fight.CurrentZone.ZoneName} - {fight.BossName}");
            Log("---------------------------------------------");
            foreach (var kvp in stats.OrderBy(i => i.Value).Reverse())
            {
                i++;
                Log($"{i}. {kvp.Key}: {kvp.Value.ToString("N")} ({(Math.Round(kvp.Value / (double)total, 2) * 100).ToString().PadRight(3).Substring(0, 3).Trim() }%)");
            }
        }

        public void Report<T, G>(Dictionary<T, Dictionary<G, long>> stats, string name, MonitoredFight fight, CombatState state)
        {
            var ts = fight.FightEnd.Subtract(fight.FightStart);
            long total = 0;
            Dictionary<T, long> totals = new Dictionary<T, long>();
            int i = 0;

            foreach (var baseKvp in stats)
            {
                var subTotal = baseKvp.Value.Sum(kvp => kvp.Value);
                total += subTotal;
                totals[baseKvp.Key] = subTotal;
            }

            Log("---------------------------------------------");
            Log($"{name}: {fight.CurrentZone.ZoneName} - {fight.BossName}");
            Log("---------------------------------------------");
            foreach (var baseKvp in totals.OrderBy(i => i.Value).Reverse())
            {
                i++;
                var j = 0;
                Log($"{i}. {baseKvp.Key}: {baseKvp.Value.ToString("N")} ({(Math.Round(baseKvp.Value / (double)total, 2) * 100).ToString().PadRight(3).Substring(0, 3).Trim() }%)");

                foreach (var kvp in stats[baseKvp.Key].OrderBy(i => i.Value).Reverse())
                {
                    j++;
                    Log($"      {j}. {kvp.Key}: {kvp.Value.ToString("N")} ({(Math.Round(kvp.Value / (double)baseKvp.Value, 2) * 100).ToString().PadRight(3).Substring(0, 3).Trim() }%)");
                }
            }
        }

        public void Report<T, G, B>(Dictionary<T, Dictionary<G, Dictionary<B, long>>> stats, string name, MonitoredFight fight, CombatState state)
        {
            var ts = fight.FightEnd.Subtract(fight.FightStart);
            long total = 0;
            Dictionary<T, long> totals = new Dictionary<T, long>();
            Dictionary<T, Dictionary<G, long>> subtotals = new Dictionary<T, Dictionary<G, long>>();
            int i = 0;

            foreach (var baseKvp in stats)
            {
                long thisTotal = 0;

                foreach (var nextKvp in baseKvp.Value)
                {
                    var subTotal = nextKvp.Value.Sum(kvp => kvp.Value);
                    thisTotal += subTotal;
                    subtotals.AddValue(baseKvp.Key, nextKvp.Key, subTotal);
                }

                total += thisTotal;
                totals[baseKvp.Key] = thisTotal;
            }

            Log("---------------------------------------------");
            Log($"{name}: {fight.CurrentZone.ZoneName} - {fight.BossName}");
            Log("---------------------------------------------");
            foreach (var baseKvp in totals.OrderBy(i => i.Value).Reverse())
            {
                i++;
                var j = 0;
                Log($"{i}. {baseKvp.Key}: {baseKvp.Value.ToString("N")} ({(Math.Round(baseKvp.Value / (double)total, 2) * 100).ToString().PadRight(3).Substring(0, 3).Trim() }%)");

                foreach (var kvp in subtotals[baseKvp.Key].OrderBy(i => i.Value).Reverse())
                {
                    j++;
                    Log($"  {j}. {kvp.Key}: {kvp.Value.ToString("N")} ({(Math.Round(kvp.Value / (double)baseKvp.Value, 2) * 100).ToString().PadRight(3).Substring(0, 3).Trim() }%)");

                    var k = 0;
                    foreach (var subkvp in stats[baseKvp.Key][kvp.Key].OrderBy(i => i.Value).Reverse())
                    {
                        k++;
                        Log($"      {k}. {subkvp.Key}: {subkvp.Value.ToString("N")} ({(Math.Round(subkvp.Value / (double)kvp.Value, 2) * 100).ToString().PadRight(3).Substring(0, 3).Trim() }%)");
                    }
                }
            }
        }

        public void ReportTable(List<List<string>> table, string name, MonitoredFight fight, CombatState state)
        {
            Log("---------------------------------------------");
            Log($"{name}: {fight.CurrentZone.ZoneName} - {fight.BossName}");
            Log("---------------------------------------------");
            foreach(var line in table)
            {
                StringBuilder sb = new StringBuilder();
                bool cellOne = true;
                foreach (var cell in line)
                {
                    sb.Append("|");
                    if (cellOne)
                    {
                        cellOne = false;
                        sb.Append(cell.PadRight(20));
                    }
                    else
                        sb.Append(cell.PadRight(10));
                }
                sb.Append("|");
                Log(sb.ToString());
            }
        }
    }
}

