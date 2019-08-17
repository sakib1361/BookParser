using System;
using System.Collections.Generic;
using System.Text;

namespace ParserEngine.Engine
{
    public static class LogEngine
    {
        public static event EventHandler<string> ErrorOccured;
        public static event EventHandler<string> InfoOccured;

        internal static void Error(string error)
        {
            ErrorOccured?.Invoke(null,error);
        }

        internal static void Data(string data)
        {
            InfoOccured?.Invoke(null, data);
        }
    }
}
