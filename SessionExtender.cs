using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace SessionExtensions{
    public static class SessionExtensions {
        //Sets JSON object in Session
        public static void SetObjectAsJson (this ISession session, string key, object value) {
            session.SetString (key, JsonConvert.SerializeObject (value));
        }

        //Gets JSON objects from Session
        public static T GetObjectFromJson<T> (this ISession session, string key) {
            string value = session.GetString (key);
            return value == null ? default (T) : JsonConvert.DeserializeObject<T> (value);
        }
    }
}