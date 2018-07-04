using System;
using System.Collections.Generic;
using System.Text;

namespace Ent.Framework.Log.LogProviders
{
    public class LibLogException : Exception
    {
        public LibLogException(string message)
            : base(message)
        {
        }

        public LibLogException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
