
using Ent.Framework.Ioc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ent.Framework.Log
{
    public static class LogInit
    {
        public static void Init()
        {
            ILogService service = ServiceLocator.GetInstance<ILogService>();
            service.InitLog(LogType.Log4net);
        }
    }
}
