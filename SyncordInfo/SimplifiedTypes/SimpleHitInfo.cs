namespace SyncordInfo.SimplifiedTypes
{
    public struct SimpleHitInfo
    {
        public int Tool { get; set; }
        public int Time { get; set; }
        public string Attacker { get; set; }
        public float Amount { get; set; }
        public SimpleDamageType DamageType { get; set; }
    }
}
