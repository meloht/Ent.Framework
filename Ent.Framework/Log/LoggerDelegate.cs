using System;
using System.Collections.Generic;
using System.Text;

namespace Ent.Framework.Log
{
#if !LIBLOG_PROVIDERS_ONLY || LIBLOG_PUBLIC
    public
#else
    internal
#endif
        delegate bool Logger(LogLevel logLevel, Func<string> messageFunc, Exception exception = null, params object[] formatParameters);
}
