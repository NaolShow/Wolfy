using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Wolfy.Classes;
using Wolfy.Classes.Recognition;
using Wolfy.Windows.ProfilesWindows;

namespace Wolfy.Windows {

    public partial class Main : Window {
        public Main() {
            InitializeComponent();

            // Logs autoscroll
            LogsTxt.TextChanged += delegate {
                LogsTxt.ScrollToEnd();
            };

            #region Profiles menu

            // Open menu
            MoreBtn.Click += delegate {

                // Translate items (don't work in XAML)
                CreateBtn.Header = Langs.Get("create_profile");
                RemoveBtn.Header = Langs.Get("remove_profile");

                MoreBtn.ContextMenu.IsOpen = true;
            };

            // Create | Remove
            CreateBtn.Click += delegate {

                // Profile informations
                string _ProfilePath = Utils.GetValidFileID(true, Reference.ProfilesPath, Langs.Get("default_profile_name"), 0);
                string _ProfileName = Path.GetFileNameWithoutExtension(_ProfilePath);

                // Create profile
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

            #endregion

            #region Top menu

            EditProfileBtn.Click += delegate {

                string _ProfileName = Profiles.GetProfile();

                // A profile is selected
                if (_ProfileName != null) {

                    // Profile exist
                    if (Profiles.Exist(_ProfileName)) {

                        // Disable recognition
                        Boolean _Rec = SpeechRecognition.State;
                        SpeechRecognition.State = false;

                        // Window
                        ProfileWindow _ProfileWindow = new ProfileWindow(_ProfileName);
                        _ProfileWindow.ShowDialog();

                        // Recognition
                        SpeechRecognition.State = _Rec;

                    } else {
                        Utils.Log(string.Format(Langs.Get("profile_doesnt_exist"), _ProfileName));
                        Profiles.RefreshList(false);
                    }

                } else {
                    Utils.Log(string.Format(Langs.Get("no_profile")));
                    Profiles.RefreshList(false);
                }

            };

            SettingsBtn.Click += delegate {

                // Window
                SettingsWindow _Settings = new SettingsWindow();
                _Settings.ShowDialog();

            };

            MicrophoneBtn.Click += delegate {

                // If profile has commands
                if (Profiles.GetProfileCommands().Count() > 0) {

                    // Switch microphone state
                    SpeechRecognition.State = !SpeechRecognition.State;
                    Utils.Log(Langs.Get((SpeechRecognition.State) ? "microphone_enabled" : "microphone_disabled"));

                } else {
                    Utils.Log(Langs.Get("microphone_no_commands"));
                }

            };

            StopCommandsBtn.Click += delegate {

                // Stop text to speech
                Classes.Recognition.Synthesizer.SpeechSynthesizer.SpeakAsyncCancelAll();
                Utils.Log(Langs.Get("commands_stopped"));

            };

            #endregion

        }

        // Needed to close the app process
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            Utils.CloseApplication();
        }

    }
}
