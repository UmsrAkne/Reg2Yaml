using System;
using System.IO;

namespace Reg2Yaml.Utils
{
    public static class Logger
    {
        private static string logFilePath;

        public static void Initialize(string logPath)
        {
            logFilePath = Path.Combine(logPath, "log.txt");
        }

        public static void Log(string message)
        {
            if (string.IsNullOrWhiteSpace(logFilePath))
            {
                logFilePath = Path.Combine(AppContext.BaseDirectory, "log.txt");
            }

            var line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {message}";

            Console.WriteLine(line);
            File.AppendAllText(logFilePath, line + Environment.NewLine);
        }
    }
}