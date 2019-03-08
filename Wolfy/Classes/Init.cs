using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Wolfy.Classes.Recognition;
using Wolfy.Windows;

namespace Wolfy.Classes {
    public static class Init {

        public static void Start() {

            // ----------------| Folders/Files |---------------- //
            Folders();
            Files();
            // ----------------| Settings |---------------- //
            Settings.Init();
            // ----------------| Lang |---------------- //
            Langs.Init();

            // ----------------| Visuals |---------------- //
            SkinManager.ApplySkin();

            // ----------------| Check for updates |---------------- //
            Update.CheckForUpdates();

            // Load all windows to be applied by the translator
            Reference.MainWindow = new Main();

            // ----------------| Recognition |---------------- //
            Synthesizer.Init();
            SpeechRecognition.Init();

            // ----------------| Profiles | ---------------- //
            Profiles.Init();

            // Hide SplashScreen
            Application.Current.MainWindow.Hide();
            // Show MainWindow
            Reference.MainWindow.Show();

            Utils.Log(Langs.Get("wolfy_loaded"));

        }

        #region Functions

        // Creates the files needed to start up
        private static void Files() {

            // Checks if files need to be created
            if (Reference.Files.Count() > 0) {
                foreach (KeyValuePair<String, String> _Pair in Reference.Files) {
                    if (!String.IsNullOrEmpty(_Pair.Key) && !String.IsNullOrEmpty(_Pair.Value))
                        File.WriteAllText(_Pair.Key, _Pair.Value);
                }
            }

        }

        // Creates the folders needed to start up
        private static void Folders() {

            // Checks if folders need to be created
            if (Reference.Folders.Count() > 0) {
                foreach (String _Folder in Reference.Folders) {
                    if (!String.IsNullOrEmpty(_Folder))
                        Directory.CreateDirectory(_Folder);
                }
            }

        }

        #endregion

    }
}
