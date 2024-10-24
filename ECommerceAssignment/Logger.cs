namespace ECommerceAssignment
{
    using System;
    using System.IO;

    public static class Logger
    {
        private static readonly string logFilePath = "C:\\Users\\Coditas-Admin\\source\\ECommerceConsoleApplication\\ECommerceAssignment\\app_log.txt";

        public static void LogError(string message)
        {
            Log("ERROR", message);
        }

        public static void LogInfo(string message)
        {
            Log("INFO", message);
        }

        private static void Log(string logType, string message)
        {
            try
            {
                using (StreamWriter writer = new(logFilePath, true))
                {
                    string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logType}] {message}";
                    writer.WriteLine(logEntry);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write to log file: {ex.Message}");
            }
        }
    }

}
