using System;
using Microsoft.Extensions.Logging;

namespace RemaWareHouse.Exceptions.Loggers
{
    public class ExceptionLogger: IExceptionLogger
    {
        public void LogException(Exception exception, string controllerName, ILogger logger)
        {
            string message = $"Following exception was thrown in {controllerName}: {exception.Message}";
            logger.LogError(message, exception);
        }
    }
}