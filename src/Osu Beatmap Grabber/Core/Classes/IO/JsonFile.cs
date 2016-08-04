using System;
using System.Collections.Generic;
using System.IO;

namespace Osu_Beatmap_Grabber.Core.Classes.IO
{
    /// <summary>
    /// Utilities class for configuration files (JSON format)
    /// </summary>
    internal class JsonFile
    {
        private static readonly Lazy<JsonFile> lazy = new Lazy<JsonFile>(() => new JsonFile());

        /// <summary>
        /// Returns the Instance of this class
        /// </summary>
        public static JsonFile Instance { get { return lazy.Value; } }

        /// <summary>
        /// Make the constructor private!
        /// </summary>
        private JsonFile() { }

        private bool Exists(string configurationName)
        {
            return File.Exists(configurationName);
        }

        /// <summary>
        /// check if a configuration exists - otherwise it will saved with given default configuration
        /// </summary>
        /// <typeparam name="T">configuration type</typeparam>
        /// <param name="configurationName">file where the config lies</param>
        /// <param name="defaultConfiguration">default configuration</param>
        public void ExistsOrCreateDefault<T>(string configurationName, T defaultConfiguration)
        {
            if (Exists(configurationName)) return;
            
            Write(defaultConfiguration, configurationName);
        }

        /// <summary>
        /// save a given configuration
        /// </summary>
        /// <typeparam name="T">configuration type</typeparam>
        /// <param name="configuration">the configuration object</param>
        /// <param name="configurationName">file to save to</param>
        public void Write<T>(T configuration, string configurationName)
        {
            JsonObject.Instance.WriteObject(configuration, configurationName);
        }

        /// <summary>
        /// load a configuration
        /// </summary>
        /// <typeparam name="T">configuration type</typeparam>
        /// <param name="configurationName">file to read from</param>
        /// <returns></returns>
        public T Read<T>(string configurationName)
        {
            if (!Exists(configurationName)) throw new System.IO.FileNotFoundException();
            T configuration = JsonObject.Instance.ReadObject<T>(configurationName);

            if (EqualityComparer<T>.Default.Equals(configuration, default(T))) throw new Newtonsoft.Json.JsonSerializationException();
            return configuration;
        }
    }
}
