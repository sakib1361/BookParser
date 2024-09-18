using System.Collections.ObjectModel;

namespace ParserEngine.Engine
{
    public static class LogEngine
    {
        private static ObservableCollection<string> _loggerData;

        public static void Init(ObservableCollection<string> loggers)
        {
            _loggerData = loggers;
        }

        internal static void Error(string error)
        {
            _loggerData?.Insert(0, $"Error {error}");
        }

        internal static void Data(string data)
        {
            _loggerData?.Insert(0, data);
        }

        public static void Clear()
        {
            _loggerData?.Clear();
        }
    }
}
