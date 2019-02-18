using Wolfy.Properties;
using Wolfy.Files.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Globalization;
using System.Threading;

namespace Wolfy.Classes {
    public static class Settings {

        /// <summary>
        /// Load the module managing the settings
        /// </summary>
        public static void Init() {

            // In case there is a format problem directly in the resources. 
            // (And so even when rewriting settings, it's always bad.)
            while (true) {

                // Init settings file
                if (!File.Exists(Reference.Settings)) {
                    File.WriteAllText(Reference.Settings, Resources.settings);
                }
                if (!Utils.IsValidJson(File.ReadAllText(Reference.Settings))) {
                    if (MessageBox.Show(Translation.Get("settings_file_error"), Reference.AppName, MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes) {
                        File.WriteAllText(Reference.Settings, Resources.settings);
                    }
                    else {
                        Utils.CloseApplication();
                    }
                }
                else {
                    break;
                }

            }

            // Update settings
            Utils.MergeJson(File.ReadAllText(Reference.Settings), Resources.settings);

            // Load settings as Json
            Reference.JsonSettings = JsonConvert.DeserializeObject<JsonSettings>(File.ReadAllText(Reference.Settings));

            // Recover user's language
            if (Reference.JsonSettings.Language == null) {
                CultureInfo _currentCulture = Thread.CurrentThread.CurrentCulture;
                // Check if language file exist
                if (File.Exists(Reference.LangsPath + _currentCulture.TwoLetterISOLanguageName + ".json")) {
                    Reference.JsonSettings.Language = _currentCulture.TwoLetterISOLanguageName;
                }
                else {
                    // If not, set English as default language
                    Reference.JsonSettings.Language = "en";
                }
            }

        }

    }
}
