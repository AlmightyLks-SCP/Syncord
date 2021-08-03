using Newtonsoft.Json;
using System;

namespace SyncordInfo.Helper
{

    public static class JsonExtension
    {
        public static string Serialize<T>(this T obj)
            => JsonConvert.SerializeObject(obj);
        public static T Deserialize<T>(this string jsonStr)
            => JsonConvert.DeserializeObject<T>(jsonStr);

        public static bool TryDeserializeJson<T>(this string jsonStr, out T result)
        {
            result = default;
            bool success = true;
            try
            {
                result = Deserialize<T>(jsonStr);
            }
            catch (Exception e)
            {
                success = false;
            }
            return success && result != null;
        }
    }
}
