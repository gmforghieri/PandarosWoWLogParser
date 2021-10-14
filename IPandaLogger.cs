using System;

namespace PandarosWoWLogParser
{
    public interface IPandaLogger
    {
        void Log(string message, params object[] args);
        void LogError(Exception e);
        void LogError(Exception e, string message);
        void LogError(Exception e, string message, params object[] args);
    }
}