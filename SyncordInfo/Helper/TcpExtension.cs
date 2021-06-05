using Newtonsoft.Json;
using SimpleTcp;

namespace SyncordInfo.Helper
{
    public static class TcpExtension
    {
        public static void SendAsJson<T>(this SimpleTcpClient client, T obj)
            => client.Send(JsonConvert.SerializeObject(obj, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All }));
        public static void SendAsJson<T>(this SimpleTcpServer server, string ipPort, T obj)
            => server.Send(ipPort, JsonConvert.SerializeObject(obj, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All }));
    }
}
