using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using System.Windows;
using Wolfy.Files.Json;

namespace Wolfy.Classes {
    public static class Settings {

        public static void Init() {

            #region Init settings

            // Rewrite
            bool _Rewrite = false;

            // Settings file doesn't exist
            if (!File.Exists(Reference.Settings)) {
                _Rewrite = true;
            } else {

                // Settings file isn't valid
                if (!Utils.IsValidJson(File.ReadAllText(Reference.Settings))) {
                    if (MessageBox.Show("The settings file is not in a correct/readable format.\n Do you want to reset your settings?", Reference.AppName, MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes) {
                        _Rewrite = true;
                    } else {
                        Environment.Exit(0);
                    }
                }

            }

            // Rewrite settings file
            if (_Rewrite) {
                File.WriteAllText(Reference.Settings, JsonConvert.SerializeObject(new JsonSettings(), Formatting.Indented));
            }

            #endregion

            #region Load settings

            // Load settings
            Reference.JsonSettings = JsonConvert.DeserializeObject<JsonSettings>(Utils.MergeJson(File.ReadAllText(Reference.Settings), JsonConvert.SerializeObject(new JsonSettings(), Formatting.Indented)));

            // Get user language (if it's not set)
            if (Reference.JsonSettings.Language == null) {

                string _Lang = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                MessageBox.Show(_Lang);
                // Wolfy handle this language
                if (File.Exists(Reference.LangsPath + _Lang + ".xaml")) {
                    Reference.JsonSettings.Language = _Lang;
                }
                // Else, leave default language
                else {
                    Reference.JsonSettings.Language = "en";
                }
            }

            #endregion

        }

    }
}
