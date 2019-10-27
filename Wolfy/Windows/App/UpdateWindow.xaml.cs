using System;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using System.Windows;
using Wolfy.Classes;
using Wolfy.Classes.App;
using Wolfy.Json;

namespace Wolfy.Windows.App {
    /// <summary>
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window {

        private static readonly WebClient Downloader = new WebClient();
        private static JUpdate Update;

        /// <summary>
        /// Start download the update
        /// </summary>
        private void Start() {
            Downloader.DownloadFileAsync(new Uri(Update.Url), Reference.Files.Update);
        }

        /// <summary>
        /// Update window (will download and start update installation)
        /// </summary>
        public UpdateWindow(JUpdate update) {
            InitializeComponent();

            Update = update;

            // ---- Events

            // Close
            CancelBtn.Click += (sender, e) => { Utils.General.RestartApplication(); };
            this.Closing += (sender, e) => { Utils.General.RestartApplication(); };

            // Download
            Downloader.DownloadFileCompleted += (sender, e) => {

                // Download hasn't been canceled
                if (!e.Cancelled) {

                    // Downloaded file checksum equals the remote file (so the file isn't corrupted)
                    if (Utils.Files.GetFileChecksum(Reference.Files.Update, new SHA256CryptoServiceProvider()).Equals(Update.Checksum)) {

                        // Start installer
                        Process.Start(Reference.Files.Update, string.Format("/SP- /SILENT /NORESTART /NOCANCEL /FORCECLOSEAPPLICATIONS /DIR=\"{0}\" {1}", Reference.AppPath, (Reference.IsPortable) ? "/TASKS=\"portable\"" : null));
                        // Close app
                        Environment.Exit(0);

                    } else {

                        // User doesn't want to restart update
                        if (MessageBox.Show(Langs.Get("update_error"), Reference.AppName, MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.No) {
                            Utils.General.RestartApplication();
                        }
                        Start();

                    }

                }

            };
            Downloader.DownloadProgressChanged += (sender, e) => {

                // Window title
                this.Title = String.Format(Langs.Get("update_downloading"), Utils.Files.BytesToString(e.BytesReceived), Utils.Files.BytesToString(e.TotalBytesToReceive), e.ProgressPercentage);
                // Progress bar
                ProgressBar.Value = e.ProgressPercentage;

            };

            // ---- Start

            Start();

        }

    }
}
