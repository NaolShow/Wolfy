using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Windows;

namespace Wolfy.Classes {
    public static class Update {

        /// <summary>
        /// Checks if a new version is available. 
        /// If a new version is available, ask the user if he wants to install it, otherwise do nothing
        /// </summary>
        public static bool CheckForUpdates(bool _AskToInstall) {

            try {

                // Delete temporary files
                if (Directory.Exists(Reference.TempUpdatePath)) {
                    Directory.Delete(Reference.TempUpdatePath, true);
                    File.Delete(Reference.TempUpdateZip);
                }

                // Retrieve update informations
                WebClient _Client = new WebClient();
                var _JsonDefinition = new {
                    Latest_link = "",
                    Latest_version = ""
                };
                string _JsonText = _Client.DownloadString(Reference.UpdateLink);
                var _Json = JsonConvert.DeserializeAnonymousType(_JsonText, _JsonDefinition);

                // Update is valid
                if (Reference.AppVersion != _Json.Latest_version && Utils.IsValidJson(_JsonText)) {

                    // Ask user to install the update
                    if (_AskToInstall) {
                        AskToInstall(_Json.Latest_link);
                    }

                    // Return
                    return true;

                }

            } catch {
                MessageBox.Show(Langs.Get("update_check_error"), Reference.AppName, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return false;
        }

        private static void AskToInstall(string _UpdateLink) {

            // User want to install the update
            if (MessageBox.Show(Langs.Get("update_request"), Reference.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {

                // Hide splashscreen
                App.Current.MainWindow.Hide();

                // Show update window
                Windows.Update _Update = new Windows.Update(_UpdateLink);
                _Update.ShowDialog();

            }

        }

    }
}
