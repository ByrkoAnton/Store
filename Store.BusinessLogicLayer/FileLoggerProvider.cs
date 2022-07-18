﻿using Microsoft.Extensions.Logging;

namespace Store.BusinessLogicLayer
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private string path;//TODO wrong code style
        public FileLoggerProvider(string _path) //TODO wrong code style
        {
            path = _path;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(path);
        }

        public void Dispose()
        {
        }
    }
}
