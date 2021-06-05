
namespace SyncordInfo.Communication
{
    public sealed class Response : DataBase
    {
        public Query Query { get; set; }
        public string Content { get; set; }
    }
}
