using Microsoft.Extensions.Logging;

namespace Store.BusinessLogicLayer
{
    public static class FileLoggerExtention
    {
        public static ILoggerFactory AddFile(this ILoggerFactory factory, string filePath)
        {
            factory.AddProvider(new FileLoggerProvider(filePath));
            return factory;
        }
    }
}
