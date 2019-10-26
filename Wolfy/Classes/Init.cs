using System;
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
            if (Reference.Folders.Count() > 0) {

                // Create folders
                foreach (String _Folder in Reference.Folders) {
                    if (!String.IsNullOrEmpty(_Folder)) {
                        Directory.CreateDirectory(_Folder);
                    }
                }

            }

            // IronPythonLib
            File.WriteAllBytes(Reference.IronModules, Resources.IronPythonLib);

            // |-------[ Settings ]-------| //
            Settings.Init();
            // |-------[ Languages ]-------| //
            Langs.Init();

            // |-------[ Visual ]-------| //
            SkinManager.ApplySkin();

            // |-------[ Updates ]-------| //

            // Delete temporary files
            Update.DeleteTemporaryFiles();

            // Update is available
            if (Reference.JsonSettings.Check_for_updates && Update.UpdateAvaible()) {

                // Ask user to install it, and then install
                if (Update.InstallUpdate(true)) {
                    // User install the update, stop execution
                    return;
                }

            }

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
