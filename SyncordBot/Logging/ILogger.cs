namespace SyncordBot.Logging
{
    internal interface ILogger
    {
        public void Info(string data);
        public void Error(string data);
        public void Warn(string data);
        public void Exception(string data);
        public void FileLogExceptions(int amount = 10);
    }
}