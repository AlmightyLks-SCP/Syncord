extern alias NuGetUtf8Json;
using Foo = NuGetUtf8Json.Utf8Json.JsonSerializer;

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
            result = default;
            bool success = true;
            try
            {
                result = Foo.Deserialize<T>(jsonStr);
            }
            catch (Exception e)
            {
                success = false;
            }
            return success && result != null;
        }
    }
}
