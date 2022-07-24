using Microsoft.Extensions.Logging;

namespace Store.BusinessLogicLayer
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private string _path;//TODO wrong code style+++
        public FileLoggerProvider(string path) //TODO wrong code style+++
        {
            _path = path;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(_path);
        }

        public void Dispose()
        {
        }
    }
}
