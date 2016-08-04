using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osu_Beatmap_Grabber.Updater.Classes
{

    /// <summary>
    /// Updater Controlling Class
    /// Use the Instance of this class as interface for all updating operations
    /// 
    /// You have to call Init() to use all functions!
    /// </summary>
    public sealed class Updater {
        private static readonly Lazy<Updater> lazy = new Lazy<Updater>(() => new Updater());

        /// <summary>
        /// Returns the Instance of this class
        /// </summary>
        public static Updater Instance { get { return lazy.Value; } }

        private Structures.Configuration _defaultConfiguration = new Structures.Configuration
        {
            AutoUpdate = true,
            CurrentVersion = "0",
            Domain = new Uri("http://sources.kagu-chan.de"),
            Project = string.Empty,
            ProtectedDirectories = new string[] { "_pdata" },
            UpdateDirectory = "_udata"
        };

        private string _defaultConfigurationName = "_kcConfig.json";

        private bool _initialized = false;

        /// <summary>
        /// Make the constructor private!
        /// </summary>
        private Updater() { }

        /// <summary>
        /// Initialize the updater with default configuration and default configuration file
        /// </summary>
        public void Init()
        {
            Osu_Beatmap_Grabber.Core.Classes.IO.JsonFile.Instance.ExistsOrCreateDefault(_defaultConfigurationName, _defaultConfiguration);

            _initialized = true;
        }

        /// <summary>
        /// Initialize the updater with default configuration
        /// </summary>
        /// <param name="configurationName">new default configuration file</param>
        public void Init(string configurationName)
        {
            _defaultConfigurationName = configurationName;
            Init();
        }

        /// <summary>
        /// Initialize the updater with default configuration file
        /// </summary>
        /// <param name="configuration">new default configuration</param>
        public void Init(Structures.Configuration configuration)
        {
            _defaultConfiguration = configuration;
            Init();
        }

        /// <summary>
        /// Initialize the updater with completely overwritten defaults
        /// </summary>
        /// <param name="configuration">new default configuration</param>
        /// <param name="configurationName">new default congiguration file</param>
        public void Init(Structures.Configuration configuration, string configurationName)
        {
            _defaultConfiguration = configuration;
            _defaultConfigurationName = configurationName;
            Init();
        }

        /// <summary>
        /// read the current configuration
        /// </summary>
        /// <returns></returns>
        public Structures.Configuration Configuration()
        {
            if (!_initialized) throw new Exceptions.UpdaterNotInitializedException();
            return Osu_Beatmap_Grabber.Core.Classes.IO.JsonFile.Instance.Read<Structures.Configuration>(_defaultConfigurationName);
        }

        /// <summary>
        /// save a new configuration
        /// </summary>
        /// <param name="configuration">new configuration</param>
        /// <returns></returns>
        public Structures.Configuration Configuration(Structures.Configuration configuration)
        {
            if (!_initialized) throw new Exceptions.UpdaterNotInitializedException();
            Osu_Beatmap_Grabber.Core.Classes.IO.JsonFile.Instance.Write(configuration, _defaultConfigurationName);
            return Osu_Beatmap_Grabber.Core.Classes.IO.JsonFile.Instance.Read<Structures.Configuration>(_defaultConfigurationName);
        }

        /// <summary>
        /// force the update
        /// </summary>
        /// <param name="handler">updatehandler for updating</param>
        /// <returns>is all succeeds?</returns>
        public bool ProcessUpdate(Interfaces.IUpdateHandler handler)
        {
            return ProcessUpdate(handler, true);
        }

        /// <summary>
        /// process the update depending on forceUpdate and AutoUpdate setting
        /// </summary>
        /// <param name="handler">updatehandler for updating</param>
        /// <param name="forceUpdate">should the update get forced?</param>
        /// <returns>is all succeeds?</returns>
        public bool ProcessUpdate(Interfaces.IUpdateHandler handler, bool forceUpdate)
        {
            if (!_initialized) throw new Exceptions.UpdaterNotInitializedException();

            Structures.Configuration configuration = Configuration();

            if (System.IO.Directory.Exists(configuration.UpdateDirectory))
                System.IO.Directory.Delete(configuration.UpdateDirectory, true);

            if (!handler.RetriveProjectInformations(configuration)) return false;
            if (!handler.IsUpdateRequired(configuration, forceUpdate)) return true;
            if (!handler.FetchFileList(configuration)) return false;
            if (!handler.RehashDownload(configuration)) return false;
            if (!handler.DownloadUpdate(configuration)) return false;

            while (Osu_Beatmap_Grabber.Core.Classes.Web.Downloader.Instance.Downloading) { }

            if (!handler.RehashUpdate(configuration, _defaultConfigurationName)) return false;

            configuration.CurrentVersion = handler.GetNewVersionNumber();
            Configuration(configuration);

            return handler.StopApplication(configuration);
        }

    }
}
