using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SyncordBot.Logging
{
    public sealed class Logger : ILogger
    {
        public List<string> Exceptions { get; set; }
        public Logger()
        {
            Exceptions = new List<string>();
        }

        public void Info(string data)
        {
            Console.ResetColor();
            Console.WriteLine(data);
        }
        public void Error(string data)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(data);
            Console.ResetColor();
        }
        public void Warn(string data)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(data);
            Console.ResetColor();
        }
        public void Exception(string data)
        {
            Exceptions.Add($"{DateTime.UtcNow.ToShortDateString()} {DateTime.UtcNow.ToLongTimeString()} | {data}");
        }
        public void FileLogExceptions(int amount = 30)
        {
            string dateTimeString = $"{DateTime.UtcNow.ToShortDateString().Replace('/', '-')}--{DateTime.UtcNow.ToLongTimeString().Replace(':', '-')}.txt";
            var smth = Math.Clamp(Exceptions.Count - amount, 0, Exceptions.Count);
            if (Exceptions.Count == 0) //If no exceptions
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), $"Log--{dateTimeString}.txt"), "No exceptions have been logged.");
            else //If the amount of exceptions are not more than occured
                File.WriteAllLines(Path.Combine(Directory.GetCurrentDirectory(), $"Log--{dateTimeString}.txt"), Exceptions.GetRange(Exceptions.Count - Math.Clamp(amount, 1, Exceptions.Count), Math.Clamp(amount, 1, Exceptions.Count)));
        }
    }
}
