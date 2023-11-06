using NLog;
using Project.Entity.Enums;
using Project.Services.Abstract; 

namespace Project.Services
{
    public class LoggerService : ILoggerService
    {
        private static ILogger _logger = LogManager.GetCurrentClassLogger();
        public void Log(string message, LogType type)
        {
            if(type == LogType.Error)
                _logger.Error(message);

            if (type == LogType.Info)
                _logger.Info(message);

            //if (type == LogType.Success)
            //    _logger.Info(message);

            if (type ==  LogType.Debug)
                _logger.Debug(message);

            if (type == LogType.Warn)
                _logger.Warn(message);
             
        }
    }
}
