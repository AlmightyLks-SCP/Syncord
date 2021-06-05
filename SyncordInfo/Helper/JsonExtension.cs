using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncordInfo.Helper
{

    public static class JsonExtension
    {
        public static bool TryDeserializeJson<T>(this string jsonStr, out T result)
        {
            Type type = typeof(T);
            bool success = jsonStr.TryDeserializeJson(out object obj, type);
            result = (T)obj;
            return success;
        }
        public static bool TryDeserializeJson(this string jsonStr, out object result, Type type)
        {
            result = null;
            try
            {
                result = JsonConvert.DeserializeObject(jsonStr, type, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
