
namespace SyncordInfo.Communication
{
    public sealed class Response : DataBase
    {
        public QueryType QueryType { get; set; }
        public string JsonContent { get; set; }
    }
}
