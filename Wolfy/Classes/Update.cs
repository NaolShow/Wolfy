using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Windows;

namespace Wolfy.Classes {
    public static class Update {

        /// <summary>
        /// Deletes temporary files due to updates (if they exist)
        /// Returns true if there were no problems, otherwise returns false
        /// </summary>
        public static bool DeleteTemporaryFiles() {

            try {

                // Zip file
                if (File.Exists(Reference.TempUpdateZip)) { File.Delete(Reference.TempUpdateZip); }

                // Extracted directory
                if (Directory.Exists(Reference.TempUpdatePath)) { Directory.Delete(Reference.TempUpdatePath, true); }

                // Return
                return true;

            } catch { return false; }

        }

        // JsonUpdate
        private class JsonUpdate {

            public string Latest_link { get; set; }
            public string Latest_version { get; set; }

        }

        /// <summary>
        /// Return update informations
        /// </summary>
        private static JsonUpdate FetchUpdate() {

            try {

                // Update file
                string _UpdateContent = new WebClient().DownloadString(Reference.UpdateLink);

                // Update content is valid
                if (Utils.IsValidJson(_UpdateContent)) {
                    return JsonConvert.DeserializeObject<JsonUpdate>(_UpdateContent);
                }

            } catch { }

            return null;

        }

        /// <summary>
        /// Checks if a new update is available
        /// Returns true if this is the case, otherwise returns false
        /// </summary>
        public static bool UpdateAvaible() {

            try {

                // Fetch update
                JsonUpdate _Update = FetchUpdate();

                // Check if versions are not equal
                if (Reference.AppVersion != _Update.Latest_version) {
                    return true;
                }

            } catch {
                MessageBox.Show(Langs.Get("update_check_error"), Reference.AppName, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Return
            return false;

        }

        /// <summary>
        /// Installs the latest update, (make sure to check if an update is available!)
        /// If _AskUser is true, first ask the user if he wants to install it or not
        /// Returns true if the update will be installed, otherwise returns false
        /// </summary>
        public static bool InstallUpdate(bool _AskUser) {

            // User don't want to install
            if (_AskUser && MessageBox.Show(Langs.Get("update_request"), Reference.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No) {
                return false;
            }

            // Fetch update
            JsonUpdate _Update = FetchUpdate();

            // Else install the update

            // Hide splashscreen
            App.Current.MainWindow.Hide();

            // Show update window
            Windows.Update _UpdateWindow = new Windows.Update(_Update.Latest_link);
            _UpdateWindow.ShowDialog();

            // Return
            return true;

        }

    }
}
