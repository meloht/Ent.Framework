using Ent.Framework.Ioc;
using Ent.Framework.Log.Log4NetService;
using Ent.Framework.Log.NLogService;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ent.Framework.Log
{
    [ServiceImplementation]
    public class LogService : ILogService
    {
        public void InitLog(LogType logType)
        {
            switch (logType)
            {
                case LogType.Nlog:
                    var nlog = new NLogImp();
                    nlog.InitLog();
                    break;

                case LogType.Log4net:
                    var log = new Log4netImp();
                    log.InitLog();
                    break;
                default:
                    break;
            }
        }
    }
}
