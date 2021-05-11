namespace SyncordBot.Models.DTO
{
    public sealed class ScpSlServer
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string FullAddress { get; set; }
        public ScpSlServer()
        {
            Id = default;
            Name = default;
            FullAddress = default;
        }
        public ScpSlServer(string name, string fullAddress, int port)
        {
            Id = default;
            Name = name;
            FullAddress = fullAddress;
        }
        public ScpSlServer(long id, string name, string fullAddress, int port)
        {
            Id = id;
            Name = name;
            FullAddress = fullAddress;
        }
    }
}
