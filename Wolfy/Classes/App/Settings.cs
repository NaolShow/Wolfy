using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;
using Wolfy.Json;

namespace Wolfy.Classes.App {

    public static class Settings {

        /// <summary>
        /// User settings
        /// </summary>
        public static JSettings Data;
        /// <summary>
        /// Default settings
        /// </summary>
        public static JSettings DefaultData;

        /// <summary>
        /// Initialize application settings
        /// </summary>
        public static void Initialize() {

            // Default settings
            DefaultData = new JSettings();

            #region Write settings

            // Write settings ?
            bool write = false;

            // Settings file doesn't exists
            if (!File.Exists(Reference.Files.Settings)) {
                write = true;
            }
            // File isn't correct (json format)
            else if (!Utils.Json.IsValid(File.ReadAllText(Reference.Files.Settings))) {

                // User doesn't want to rewrite settings
                if (MessageBox.Show("The settings file isn't in a correct format.\nDo you want to reset your settings ?", Reference.AppName, MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.No) {
                    Environment.Exit(0);
                }
                write = true;

            }

            // Write settings
            if (write) {

                try {
                    File.WriteAllText(Reference.Files.Settings, JsonConvert.SerializeObject(DefaultData, Formatting.Indented));
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message, Reference.AppName, MessageBoxButton.OK, MessageBoxImage.Error);
                    Environment.Exit(0);
                }

            }


            #endregion

            // Load settings
            Data = JsonConvert.DeserializeObject<JSettings>(Utils.Json.Merge(File.ReadAllText(Reference.Files.Settings), JsonConvert.SerializeObject(DefaultData)));

        }

    }

}
