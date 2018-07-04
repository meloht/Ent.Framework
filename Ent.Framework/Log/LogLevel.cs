using System;
using System.Collections.Generic;
using System.Text;

namespace Ent.Framework.Log
{
    /// <summary>
    ///     The log level.
    /// </summary>
#if LIBLOG_PROVIDERS_ONLY
    internal
#else
    public
#endif
        enum LogLevel
    {
        Trace,
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }
}
