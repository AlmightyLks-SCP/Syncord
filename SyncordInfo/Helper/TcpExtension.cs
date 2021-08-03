using SimpleTcp;

namespace SyncordInfo.Helper
{
    public static class TcpExtension
    {
        public static void SendAsJson<T>(this SimpleTcpClient client, T obj)
            => client.Send(Utf8Json.JsonSerializer.Serialize(obj));
        public static void SendAsJson<T>(this SimpleTcpServer server, string ipPort, T obj)
            => server.Send(ipPort, Utf8Json.JsonSerializer.Serialize(obj));
    }
}
