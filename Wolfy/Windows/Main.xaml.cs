using Wolfy.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Wolfy.Windows {
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window, ITranslation {
        public Main() {
            InitializeComponent();
        }

        // Translation event
        public void OnTranslation() {

            // Menu items
            AddProfileBtn.Header = Translation.Get("add_profile");
            EditProfileBtn.Header = Translation.Get("edit_profile");
            RemoveProfileBtn.Header = Translation.Get("remove_profile");

        }

        // Needed to close the app process
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            Utils.CloseApplication();
        }
        
        // Auto scroll
        private void LogsTxt_TextChanged(object sender, TextChangedEventArgs e) {
            LogsTxt.ScrollToEnd();
        }

        #region Menu buttons

        // Settings button
        private void SettingsBtn_Click(object sender, RoutedEventArgs e) {

            // Show settings window
            SettingsWindow _settings = new SettingsWindow();
            Translation.TranslateWindow(_settings);
            _settings.ShowDialog();

        }

        // Microphone button
        private void MicrophoneBtn_Click(object sender, RoutedEventArgs e) {

            // If microphone is active
            if (MicrophoneIcon.Kind.ToString() == "MicrophoneVariant") {
                Classes.Recognition.SpeechRecognition.SetActive(false);
                MicrophoneIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.MicrophoneVariantOff;
                Utils.Log(Translation.Get("microphone_disabled"));
            } else {
                Classes.Recognition.SpeechRecognition.SetActive(true);
                MicrophoneIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.MicrophoneVariant;
                Utils.Log(Translation.Get("microphone_enabled"));
            }

        }

        // Stop commands
        private void StopCommands_Click(object sender, RoutedEventArgs e) {
            // Stop text to speech
            Classes.Recognition.Synthesizer.synthesizer.SpeakAsyncCancelAll();
            Utils.Log(Translation.Get("tr:commands_stopped"));
        }

        #endregion

    }
}
