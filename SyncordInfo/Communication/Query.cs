namespace SyncordInfo.Communication
{
    public sealed class Query
    {
        public QueryType QueryType { get; set; }
    }
    public enum QueryType
    {
        PlayerCount = 1,
        PlayerDeaths,
        ServerFps
    }
}
