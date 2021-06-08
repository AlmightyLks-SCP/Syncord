extern alias NuGetUtf8Json;
using Foo = NuGetUtf8Json.Utf8Json.JsonSerializer;

using SimpleTcp;

namespace SyncordInfo.Helper
{
    public static class TcpExtension
    {
        public static void SendAsJson<T>(this SimpleTcpClient client, T obj)
            => client.Send(Foo.Serialize(obj));
        public static void SendAsJson<T>(this SimpleTcpServer server, string ipPort, T obj)
            => server.Send(ipPort, Foo.Serialize(obj));
    }
}
