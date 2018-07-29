using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ent.Framework.Log.NLogService
{
    public class NLogImp
    {
        public void InitLog()
        {
            var config = new LoggingConfiguration();
            var consoleTarget = new ColoredConsoleTarget();
            config.AddTarget("console", consoleTarget);
            config.LoggingRules.Add(new LoggingRule("*", NLog.LogLevel.Debug, consoleTarget));
            LogManager.Configuration = config;
            
        }
    }
}
