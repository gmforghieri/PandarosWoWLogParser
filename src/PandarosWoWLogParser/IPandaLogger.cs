using System;

namespace PandarosWoWLogParser
{
    public interface IPandaLogger : IStatsReporter
    {
        void Log(string message, params object[] args);
        void LogError(Exception e);
        void LogError(Exception e, string message);
        void LogError(Exception e, string message, params object[] args);
    }
}