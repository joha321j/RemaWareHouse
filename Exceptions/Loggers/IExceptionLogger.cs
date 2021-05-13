using System;
using Microsoft.Extensions.Logging;

namespace RemaWareHouse.Exceptions.Loggers
{
    public interface IExceptionLogger
    {
        void LogException(Exception exception, string controllerName, ILogger logger);
    }
}