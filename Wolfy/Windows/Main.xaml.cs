using System;
using System.Windows;
using System.Windows.Controls;
using Wolfy.Classes;

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
