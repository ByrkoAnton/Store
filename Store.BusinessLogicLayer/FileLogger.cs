﻿using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Store.BusinessLogicLayer
{
    public class FileLogger : ILogger
    {
        private string _filePath;//TODO wrong code style+++
        private static object _lock = new object();
        public FileLogger(string path)
        {
            _filePath = path;
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                lock (_lock)
                {
                    File.AppendAllText(_filePath, formatter(state, exception) + Environment.NewLine);
                }
            }
        }
    }
}
