using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Wolfy.Classes;
using Wolfy.Windows.ProfilesWindows;

namespace Wolfy.Windows {

    public partial class Main : Window {
        public Main() {
            InitializeComponent();

            // ----------------| Auto scroll |---------------- //

            LogsTxt.TextChanged += delegate {
                LogsTxt.ScrollToEnd();
            };

            // ----------------| Menu |---------------- //

            // Main menu
            MoreBtn.Click += delegate {
                MoreBtn.ContextMenu.IsOpen = true;
            };

            // Create | Remove
            CreateBtn.Click += delegate {

                // Get profile
                String _ProfilePath = Utils.GetValidFileID(true, Reference.ProfilesPath, "Profile", 0);
                String _ProfileName = Path.GetFileNameWithoutExtension(_ProfilePath);

                // Create
                Directory.CreateDirectory(_ProfilePath);
                ProfilesCombo.Items.Add(new ListBoxItem() { Content = _ProfileName });

                // Select profile
                Profiles.Select(_ProfileName);

            };
            RemoveBtn.Click += delegate {

                // Confirmation
                if (MessageBox.Show(Langs.Get("remove_profile_confirm"), Reference.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {

                    // Remove profile
                    Directory.Delete(Profiles.GetProfilePath(), true);
                    // Refresh
                    Profiles.RefreshList(false);

                }

            };

        }

        #region Menu buttons

        // Edit profile
        private void EditProfileBtn_Click(object sender, RoutedEventArgs e) {

            // Check if there is a profile
            if (Profiles.GetProfile() != null && Profiles.GetProfiles().Count() > 0) {

                // Get current profile name
                String _CurrentProfile = Profiles.GetProfile();

                // Profile exist
                if (Profiles.Exist(_CurrentProfile)) {

                    // Show profile window
                    ProfileWindow _ProfileWindow = new ProfileWindow(_CurrentProfile);
                    _ProfileWindow.ShowDialog();

                } else {
                    Utils.Log(String.Format(Langs.Get("profile_doesnt_exist"), _CurrentProfile));
                    Profiles.RefreshList(false);
                }

            } else {
                Utils.Log(String.Format(Langs.Get("no_profile")));
                Profiles.RefreshList(false);
            }

        }

        // Settings button
        private void SettingsBtn_Click(object sender, RoutedEventArgs e) {

            // Show settings window
            SettingsWindow _settings = new SettingsWindow();
            _settings.ShowDialog();

        }

        // Microphone button
        private void MicrophoneBtn_Click(object sender, RoutedEventArgs e) {

            // If profile has commands
            if (Profiles.GetProfileCommands().Count() > 0) {

                // If microphone is active
                if (MicrophoneIcon.Kind.ToString() == "MicrophoneVariant") {
                    Classes.Recognition.SpeechRecognition.State = false;
                    Utils.Log(Langs.Get("microphone_disabled"));
                } else {
                    Classes.Recognition.SpeechRecognition.State = true;
                    Utils.Log(Langs.Get("microphone_enabled"));
                }

            } else {
                Utils.Log(Langs.Get("microphone_no_commands"));
            }

        }

        // Stop commands
        private void StopCommands_Click(object sender, RoutedEventArgs e) {
            // Stop text to speech
            Classes.Recognition.Synthesizer.synthesizer.SpeakAsyncCancelAll();
            Utils.Log(Langs.Get("commands_stopped"));
        }

        #endregion

        // Needed to close the app process
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            Utils.CloseApplication();
        }

    }
}
