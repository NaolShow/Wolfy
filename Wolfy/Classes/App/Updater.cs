using Newtonsoft.Json;
using System.Net;
using Wolfy.Json;
using Wolfy.Windows.App;

namespace Wolfy.Classes.App {

    public static class Updater {

        // Update file link
        private const string UpdateURL = "https://dl.dropbox.com/s/5wuh5o01q3q3oti/a";

        public static JUpdate Update { get; private set; }
        public static WebClient Downloader = new WebClient();

        /// <summary>
        /// Fetch update informations
        /// Returns false if the update informations can't be reached or are not readable
        /// </summary>
        public static bool FetchUpdate() {

            try {

                // Download update content and convert it to json
                Update = JsonConvert.DeserializeObject<JUpdate>(Downloader.DownloadString(UpdateURL));
                return true;

            } catch { Update = null; return false; }

        }

        /// <summary>
        /// Checks if there is a newer version of the software
        /// </summary>
        public static bool HasUpdate() {
            return (Update != null && Reference.AppVersion.CompareTo(Update.Version) < 0);
        }

        /// <summary>
        /// Download and install the update
        /// </summary>
        public static void Install() {

            // An update has been already fetched
            if (Update != null) {

                // Create update window
                UpdateWindow window = new UpdateWindow(Update);
                // Start downloading & installing
                window.Show();

            }

        }

    }

}
