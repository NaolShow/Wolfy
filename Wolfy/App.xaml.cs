using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using System.Windows;
using Wolfy.Classes;
using Wolfy.Classes.App;
using Wolfy.Windows;

namespace Wolfy {

    public partial class App : Application {

        /// <summary>
        /// Application start point
        /// </summary>
        protected override void OnStartup(StartupEventArgs e) {

            // ---- Command line args
            /** foreach (string arg in e.Args) {
                Console.WriteLine(arg);
            } **/

            // ---- Portable mode

            // Registry key
            RegistryKey portableKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + Reference.AppName, false);
            // Set the app as portable or not
            Reference.IsPortable = (portableKey == null || portableKey.GetValue("InstallPath", ".").ToString() != Reference.AppPath);

            // ---- Folders

            // Folders list
            string[] folders = {
                Reference.Folders.Langs
            };

            // Create
            foreach (string folder in folders) {
                Directory.CreateDirectory(folder);
            }

            // ---- Settings

            Settings.Initialize();

            // ---- Langs

            Langs.Initialize();

            // ---- Appearance

            Appearance.Initialize();

            // ---- Updater

            // User has check for updates to true
            if (Settings.Data.CheckForUpdates) {

                // User has connection and there is an update
                if (Updater.FetchUpdate() && Updater.HasUpdate()) {

                    // User wants to install the update
                    if (MessageBox.Show(string.Format(Langs.Get("update_ask"), Reference.AppVersion, Updater.Update.Version), Reference.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
                        Updater.Install();
                        return;
                    }

                }

            }

            // ---- Window

            new MainWindow().Show();

        }

        /// <summary>
        /// Application exit point
        /// </summary>
        protected override void OnExit(ExitEventArgs e) {

            // Save settings
            File.WriteAllText(Reference.Files.Settings, JsonConvert.SerializeObject(Settings.Data, Formatting.Indented));

            base.OnExit(e);

        }

    }

}
