﻿using Microsoft.Extensions.Logging;

namespace Store.BusinessLogicLayer
{
    public static class FileLoggerExtension//TODO wrong spelling+++
    {
        public static ILoggerFactory AddFile(this ILoggerFactory factory, string filePath)
        {
            factory.AddProvider(new FileLoggerProvider(filePath));
            return factory;
        }
    }
}