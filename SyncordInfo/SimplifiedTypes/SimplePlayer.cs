namespace SyncordInfo.SimplifiedTypes
{
    public class SimplePlayer
    {
        public int Ping { get; set; }
        public string DisplayName { get; set; }
        public string Nickname { get; set; }
        public string UserId { get; set; }
        public SimpleCustomRole Role { get; set; }
        public int MaxArtificialHealth { get; set; }
        public float ArtificialHealth { get; set; }
        public int MaxHealth { get; set; }
        public float Health { get; set; }
        public SimpleSynapseGroup SynapseGroup { get; set; }
        public string IPAddress { get; set; }
        public bool DoNotTrack { get; set; }
        public bool IsCuffed { get; set; }
        public SimplePlayer()
        {
            Ping = -1;
            DisplayName = string.Empty;
            Nickname = string.Empty;
            UserId = string.Empty;
            Role = new SimpleCustomRole();
            MaxArtificialHealth = -1;
            ArtificialHealth = -1;
            MaxHealth = -1;
            Health = -1;
            SynapseGroup = null;
            IPAddress = string.Empty;
            IsCuffed = false;
        }

        public static SimplePlayer Unknown
            => new SimplePlayer()
            {
                DisplayName = "Unknown",
                Nickname = "Unknown",
                IPAddress = "Unknown",
                UserId = "Unknown"
            };
    }
}
