﻿using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Osu_Beatmap_Grabber
{
    public partial class FUpdate : Form
    {
        private string _appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private string _appPath;
        private UpdateHandler _updater;

        public delegate void DSetText(string text);
        public delegate void DSetPercent(double percent);
        public delegate void DHide();

        public FUpdate()
        {
            InitializeComponent();

            BackgroundWorker bgw = new BackgroundWorker();
            bgw.DoWork += Bgw_DoWork;

            bgw.RunWorkerAsync();
        }

        public void SetText(string text)
        {
            if (LBUpdate.InvokeRequired)
            {
                LBUpdate.Invoke(new DSetText(SetText), text);
            } else LBUpdate.Text = text;
        }

        public void SetPercent(double perc)
        {
            if (PBUpdate.InvokeRequired)
            {
                PBUpdate.Invoke(new DSetPercent(SetPercent), perc);
            } else PBUpdate.Value = (int) (perc * 100);
        }

        private void Bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            _appPath = System.IO.Path.Combine(_appData, "OsuBeatmapGrabber");

            SetText("Check Application Environment...");
            
            if (!System.IO.Directory.Exists(_appPath))
            {
                System.IO.Directory.CreateDirectory(_appPath);
            }

            System.Threading.Thread.Sleep(1000);

            Osu_Beatmap_Grabber.Updater.Structures.Configuration config = new Osu_Beatmap_Grabber.Updater.Structures.Configuration();
            config.AutoUpdate = true;
            config.CurrentVersion = "0";
            config.Domain = new Uri("http://sources.kagu-chan.de");
            config.Project = "osuSongGrabber_rs2";
            config.ProtectedDirectories = new string[] { "conf", "OsuSongs" };
            config.UpdateDirectory = System.IO.Path.Combine(_appPath, "update");

            string updateConfig = System.IO.Path.Combine(_appPath, "_kcConfig.json");

            Osu_Beatmap_Grabber.Updater.Classes.Updater.Instance.Init(config, updateConfig);

            InitAppIcon();

            _updater = new UpdateHandler();
            _updater.OnLastMessageChanged += OnLastMessageChanged;
            _updater.OnUpdatingChanged += OnUpdatingChanged;
            _updater.OnProgressChanged += OnProgressChanged;

            _updater.TargetDirectory = _appPath;
            _updater.UpdateDirectory = config.UpdateDirectory;

            Osu_Beatmap_Grabber.Updater.Classes.Updater.Instance.ProcessUpdate(_updater, false);
        }

        private void InitAppIcon()
        {
            string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (appPath == _appPath) return;

            string lnkFileName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Osu Beatmap Grabber.lnk");
            Osu_Beatmap_Grabber.Core.LinkFile.Create(lnkFileName,
                System.IO.Path.Combine(_appPath, "Osu Beatmap Grabber.exe"),
                null, null, "Open osu!BeatmapGrabber", "", System.IO.Path.Combine(_appPath, "osbg.ico"));
        }

        private void OnProgressChanged(object sender, ProgressEventArgs e)
        {
            SetPercent(e.Percent);
        }

        private void OnLastMessageChanged(object sender, LastMessageEventArgs e)
        {
            SetText(e.Message);
        }

        private void OnUpdatingChanged(object sender, UpdatingEventArgs e)
        {
            if (!_updater.Updating)
            {
                SetText("Start application...");

                System.IO.Directory.SetCurrentDirectory(_appPath);
                System.Diagnostics.Process proc = System.Diagnostics.Process.Start("App.exe");
                
                Application.Exit();
            }
        }
    }
}