using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace DigitalPlus.Licensing
{
    internal static class JsonHelper
    {
        public static string Serialize<T>(T obj)
        {
            var ser = new DataContractJsonSerializer(typeof(T));
            using (var ms = new MemoryStream())
            {
                ser.WriteObject(ms, obj);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        public static T Deserialize<T>(string json)
        {
            var ser = new DataContractJsonSerializer(typeof(T));
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return (T)ser.ReadObject(ms);
            }
        }
    }
}
