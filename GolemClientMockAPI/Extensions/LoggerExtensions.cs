using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Extensions
{
    public static class LoggerExtensions
    {
        public static void LogWithProperties(this ILogger logger, LogLevel level, string nodeId, string reqProv, string subscriptionId, string message)
        {
            logger.Log(level,
                       default(EventId),
                       new MyLogEvent(message)
                            .AddProp("NodeId", nodeId)
                            .AddProp("ReqProv", reqProv)
                            .AddProp("SubscriptionId", subscriptionId),
                       (Exception)null,
                       MyLogEvent.Formatter);

        }
    }
}
