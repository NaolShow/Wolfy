using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Wolfy.Classes {
    public static class Update {

        /// <summary>
        /// Checks if a new version is available. 
        /// If a new version is available, ask the user if he wants to install it, otherwise do nothing
        /// </summary>
        /// <returns></returns>
        public static void CheckForUpdates() {

            // If an update has been made, deletes the temporary files
            if (Directory.Exists(Reference.TempUpdatePath)) {
                Directory.Delete(Reference.TempUpdatePath, true);
                File.Delete(Reference.TempUpdateZip);
            }

            // Check if "Check_for_updates" is true
            if (Reference.JsonSettings.Check_for_updates) {

                try {
                    // Retrieve update informatios
                    WebClient _Client = new WebClient();
                    var UpdateDefinition = new {
                        Latest_link = "",
                        Latest_version = ""
                    };
                    var UpdateJson = JsonConvert.DeserializeAnonymousType(_Client.DownloadString(Reference.UpdateLink), UpdateDefinition);

                    // Check version
                    if (UpdateJson.Latest_version != null && Reference.AppVersion != UpdateJson.Latest_version) {
                        // Request permission to download and install update
                        if (MessageBox.Show(Translation.Get("update_request"), Reference.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {

                            // Hide splashscreen
                            App.Current.MainWindow.Hide();

                            // Show update window
                            Windows.Update _Update = new Windows.Update(UpdateJson.Latest_link);
                            Translation.TranslateWindow(_Update);
                            _Update.ShowDialog();
                        }
                    }
                }
                catch {
                    MessageBox.Show(Translation.Get("update_check_error"), Reference.AppName, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}
