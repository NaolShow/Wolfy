using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using Wolfy.Files.Json;
using Wolfy.Properties;

namespace Wolfy.Classes {
    public static class Settings {

        public static void Init() {

            #region Init settings

            // Create settings file if it doesn't exist
            if (!File.Exists(Reference.Settings)) {
                File.WriteAllText(Reference.Settings, Resources.settings);
            }

            // Settings file content
            string _SettingsContent = File.ReadAllText(Reference.Settings);

            // Check if the current settings file isn't valid
            if (!Utils.IsValidJson(_SettingsContent)) {

                // Ask to rewrite settings if the current one is not valid
                if (MessageBox.Show(Langs.Get("settings_file_error"), Reference.AppName, MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes) {
                    File.WriteAllText(Reference.Settings, Resources.settings);
                    _SettingsContent = Resources.settings;
                }
                // Else, just leave the software
                else {
                    Utils.CloseApplication();
                }

            }

            // Update settings (add new fields..)
            Utils.MergeJson(_SettingsContent, Resources.settings);

            #endregion

            #region Load settings

            // Load settings
            Reference.JsonSettings = JsonConvert.DeserializeObject<JsonSettings>(_SettingsContent);

            // Get user language (if it's not set)
            if (Reference.JsonSettings.Language == null) {

                CultureInfo _currentCulture = Thread.CurrentThread.CurrentCulture;
                // Check if Wolfy handle this language
                if (File.Exists(Reference.LangsPath + _currentCulture.TwoLetterISOLanguageName + ".json")) {
                    Reference.JsonSettings.Language = _currentCulture.TwoLetterISOLanguageName;
                }
                // Else, just set english as default
                else {
                    Reference.JsonSettings.Language = "en";
                }
            }

            #endregion

        }

    }
}
