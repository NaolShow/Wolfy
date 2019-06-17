using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wolfy.Classes;
using Wolfy.Files.Json;

namespace Wolfy.Windows.ProfilesWindows {

    public partial class ProfileWindow : Window {

        // |-------[ Variables ]-------| //
        private static CommandBoard CurrentBoard;
        private static string Profile;

        public ProfileWindow(string _Profile) {
            InitializeComponent();

            // Title
            Profile = _Profile;
            this.Title = string.Format(Langs.Get("profile_window_title"), Profile, "?");

            // Commands init
            ReloadCommandsList();

            #region Profile name

            // Text
            ProfileName.Text = Profile;

            // Events
            ProfileName.KeyDown += (s, e) => {
                if (e.Key == Key.Enter) {
                    SaveExit();
                    this.Close();
                }
            };
            OkBtn.Click += delegate {
                SaveExit();
                this.Close();
            };

            #endregion

            #region Commands manager

            CommandMenu.Loaded += delegate {

                // No command selected
                RemoveCommand.IsEnabled = (CommandsBox.SelectedItems.Count > 0);

            };

            CreateCommand.Click += delegate {

                // Command informations
                string _CommandPath = Utils.GetValidFileID(true, Profiles.GetProfilePath(), Langs.Get("default_command_name"), 0);
                string _CommandName = Path.GetFileNameWithoutExtension(_CommandPath);

                // Command directory
                Directory.CreateDirectory(_CommandPath);
                // Command files
                File.WriteAllText(Path.Combine(_CommandPath, "command.info"), Wolfy.Properties.Resources.command_info);
                File.WriteAllText(Path.Combine(_CommandPath, "command.py"), Wolfy.Properties.Resources.command_template);

                // Reload list
                ReloadCommandsList();

            };

            RemoveCommand.Click += delegate {

                // Confirm
                if (MessageBox.Show(Langs.Get("remove_command_confirm"), Reference.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {

                    // Delete
                    Directory.Delete(((JsonCommand)((ListBoxItem)CommandsBox.SelectedItem).Tag).CommandPath, true);
                    ReloadCommandsList();

                }

            };

            #endregion

        }

        #region Commands listbox management

        /// <summary>
        /// Reload the commands list
        /// </summary>
        public void ReloadCommandsList() {
            // Clear list
            CommandsBox.Items.Clear();
            // Clear grid
            Grid.Children.Clear();
            // Clear title
            this.Title = string.Format(Langs.Get("profile_window_title"), Profile, "?");
            // Add commands
            Profiles.GetProfileCommands().ToList().ForEach(a => AddCommand(a));
        }

        /// <summary>
        /// Adds a command to the list of commands
        /// </summary>
        public void AddCommand(JsonCommand _Command) {

            // Item
            ListBoxItem _Item = new ListBoxItem() {
                Content = _Command.CommandName,
                Tag = _Command
            };

            // Event
            _Item.Selected += delegate {

                // Prevent crashing
                if (Directory.Exists(_Command.CommandPath)) {

                    // Edit title
                    this.Title = string.Format(Langs.Get("profile_window_title"), Profile, _Command.CommandName);

                    // Save selected command
                    if (CurrentBoard != null) {
                        CurrentBoard.SaveCommand();
                    }

                    // Set
                    CurrentBoard = new CommandBoard(_Command, _Item);
                    Utils.EmbedUserControl(CurrentBoard, Grid);

                }

            };

            // Add
            CommandsBox.Items.Add(_Item);

        }

        #endregion

        #region Close

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            SaveExit();
        }

        private bool IsClosing = false;
        private void SaveExit() {

            // If window is closing
            if (IsClosing) { return; }
            IsClosing = true;

            // Variables
            string _ProfileName = Profiles.GetProfile();
            string _ProfilePath = Profiles.GetProfilePath();

            // Save profile
            if (CurrentBoard != null) {
                CurrentBoard.SaveCommand();
            }

            #region Edit profile name

            // New profile name
            string _NewProfileName = Utils.GetSafeFilename(ProfileName.Text.Trim()).Trim();

            // Profile name is different & is not empty
            if (!string.IsNullOrEmpty(_NewProfileName) && _NewProfileName != _ProfileName) {

                // New profile path
                string _NewProfilePath = Path.Combine(Reference.ProfilesPath, _NewProfileName);

                // Profile doesn't exist
                if (!Directory.Exists(_NewProfilePath)) {

                    // Edit profile name
                    Directory.Move(_ProfilePath, _NewProfilePath);

                    // Profile vars
                    _ProfileName = _NewProfileName;
                    _ProfilePath = _NewProfilePath;

                } else {
                    Utils.Log(string.Format(Langs.Get("profile_already_exist"), _ProfileName));
                }
            }

            #endregion

            // Clear current board (prevent crashing)
            CurrentBoard = null;

            // Refresh list
            Profiles.RefreshList(null);
            // Select profile
            Profiles.Select(_ProfileName);

        }

        #endregion

    }
}
