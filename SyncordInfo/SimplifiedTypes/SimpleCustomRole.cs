namespace SyncordInfo.SimplifiedTypes
{
    public sealed class SimpleCustomRole
    {
        public (string Name, int Id) Role { get; set; }
        public (string Name, int Id) Team { get; set; }
        public SimpleCustomRole()
        {
            Role = (string.Empty, 0);
            Team = (string.Empty, 0);
        }
    }
}
