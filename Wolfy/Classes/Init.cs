using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Wolfy.Classes.Recognition;
using Wolfy.Properties;
using Wolfy.Windows;

namespace Wolfy.Classes {
    public static class Init {

        public static void Start() {

            // |-------[ Folders & Files ]-------| //
            Folders();
            Files();

            // IronPythonLib
            File.WriteAllBytes(Reference.IronModules, Resources.IronPythonLib);

            // |-------[ Settings ]-------| //
            Settings.Init();
            // |-------[ Languages ]-------| //
            Langs.Init();

            // |-------[ Visual ]-------| //
            SkinManager.ApplySkin();

            // |-------[ Updates ]-------| //
            if (Reference.JsonSettings.Check_for_updates) {

                if (!Update.CheckForUpdates(true)) {

                    // |-------[ Views ]-------| //
                    Reference.MainWindow = new Main();

                    // |-------[ Recognition ]-------| //
                    Synthesizer.Init();
                    SpeechRecognition.Init();

                    // |-------[ Profiles ]-------| //
                    Profiles.Init();

                    // Hide SplashScreen
                    Application.Current.MainWindow.Hide();
                    // Show MainWindow
                    Reference.MainWindow.Show();

                    Utils.Log(Langs.Get("wolfy_loaded"));

                }

            }

        }

        #region Init functions

        // Creates the files needed to start up
        private static void Files() {

            // Loop files
            if (Reference.Files.Count() > 0) {

                // Create file
                foreach (KeyValuePair<String, String> _Pair in Reference.Files) {
                    if (!String.IsNullOrEmpty(_Pair.Key) && !String.IsNullOrEmpty(_Pair.Value))
                        File.WriteAllText(_Pair.Key, _Pair.Value);
                }

            }

        }

        // Creates the folders needed to start up
        private static void Folders() {

            // Loop folders
            if (Reference.Folders.Count() > 0) {

                // Create folder
                foreach (String _Folder in Reference.Folders) {
                    if (!String.IsNullOrEmpty(_Folder))
                        Directory.CreateDirectory(_Folder);
                }

            }

        }

        #endregion

    }
}
