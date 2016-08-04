using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace Osu_Beatmap_Grabber.Core.Classes.IO
{
    /// <summary>
    /// convert objects into JSON strings and vice versa
    /// </summary>
    internal class JsonObject
    {
        private static readonly Lazy<JsonObject> lazy = new Lazy<JsonObject>(() => new JsonObject());

        /// <summary>
        /// Returns the Instance of this class
        /// </summary>
        public static JsonObject Instance { get { return lazy.Value; } }

        /// <summary>
        /// Make the constructor private!
        /// </summary>
        private JsonObject() { }

        /// <summary>
        /// write an object as JSON string into a stream
        /// </summary>
        /// <param name="streamWriter">writing stream</param>
        /// <param name="obj">object to parse</param>
        public void WriteToStream(StreamWriter streamWriter, object obj)
        {
            using (JsonTextWriter jsonWriter = new JsonTextWriter(streamWriter))
            {
                JsonSerializer serializer = new JsonSerializer();

                jsonWriter.Formatting = Formatting.Indented;
                serializer.Serialize(jsonWriter, obj);
            }
        }

        /// <summary>
        /// serialize an object as JSON string into file
        /// </summary>
        /// <param name="obj">object to serialize</param>
        /// <param name="saveTo">file to save to</param>
        /// <returns>indicates if succeed or not</returns>
        public void WriteObject(object obj, string saveTo)
        {
            using (FileStream fileStream = File.Open(saveTo, FileMode.OpenOrCreate))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    WriteToStream(streamWriter, obj);
                }
            }
        }
        
        /// <summary>
        /// deserialize JSON string from file into object
        /// </summary>
        /// <typeparam name="T">type of the new object</typeparam>
        /// <param name="readFrom">file to read from</param>
        /// <returns></returns>
        public T ReadObject<T>(string readFrom)
        {
            using (FileStream fileStream = File.Open(readFrom, FileMode.Open))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    using (JsonTextReader jsonReader = new JsonTextReader(streamReader))
                    {
                        JsonSerializer serializer = new JsonSerializer();

                        return serializer.Deserialize<T>(jsonReader);
                    }
                }
            }
        }

        /// <summary>
        /// request a JSON object from web and deserialize it
        /// </summary>
        /// <typeparam name="T">type of the new object</typeparam>
        /// <param name="uri">target domain to read from</param>
        /// <returns></returns>
        public T ReadWebObject<T>(string uri)
        {
            Web.Request.Request(uri);
            if (Web.Request.LastRequestSucceed())
            {
                return JsonConvert.DeserializeObject<T>(Web.Request.LastContent());
            } else return default(T);
        }

        public T ReadWebObject<T>(Uri uri)
        {
            return ReadWebObject<T>(uri.AbsoluteUri);
        }
    }
}
