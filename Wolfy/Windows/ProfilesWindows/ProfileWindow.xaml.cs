using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wolfy.Classes;

namespace Wolfy.Windows.ProfilesWindows {

    public partial class ProfileWindow : Window {

        public ProfileWindow(String _Profile) {
            InitializeComponent();

            // Window initialization
            this.Title = String.Format(Langs.Get("profile_window_title"), _Profile, "?");

            // Commands init
            ReloadCommandsList();

            // ----------------| Profile Name |---------------- //

            // Text
            ProfileName.Text = _Profile;

            // Event
            ProfileName.KeyDown += (s, e) => {
                if (e.Key == Key.Enter) {
                    OkBtn_Click(null, new RoutedEventArgs());
                }
            };

            // ----------------| Commands Management MENU |---------------- //

            CreateCommand.Click += delegate {

                // Get command
                String _CommandPath = Utils.GetValidFileID(false, Profiles.GetProfilePath(), "Command", 0, ".lua");

                // Create
                File.WriteAllText(_CommandPath, Wolfy.Properties.Resources.command_template);
                AddCommand(_CommandPath);

            };

            RemoveCommand.Click += delegate {

                // Command is selected
                if (CommandsBox.SelectedItems.Count > -1) {

                    // Confirm message
                    if (MessageBox.Show(Langs.Get("remove_command_confirm"), Reference.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {

                        // Delete
                        File.Delete(((ListBoxItem)CommandsBox.SelectedItem).Tag.ToString());
                        ReloadCommandsList();

                    }

                } else {
                    MessageBox.Show(Langs.Get("no_command_selected"));
                }

            };

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
            // Add commands
            Directory.GetFiles(Profiles.GetProfilePath(), "*.lua", SearchOption.TopDirectoryOnly).ToList().ForEach(a => AddCommand(a));
        }

        /// <summary>
        /// Adds a command to the list of commands
        /// </summary>
        /// <param name="_CommandPath">Command path</param>
        public void AddCommand(String _CommandPath) {

            // Item
            ListBoxItem _Item = new ListBoxItem() {
                Content = Path.GetFileNameWithoutExtension(_CommandPath),
                Tag = _CommandPath
            };

            // Event
            _Item.Selected += delegate {
                Utils.EmbedUserControl(new CommandBoard(_CommandPath), Grid);
            };

            // Add
            CommandsBox.Items.Add(_Item);

        }

        #endregion

        // Close window
        private void OkBtn_Click(object sender, RoutedEventArgs e) {

            // Change profile name
            String _ProfileName = Utils.RemoveSpecialCharacters(ProfileName.Text.Trim()).Trim();
            String _ProfilePath = Path.Combine(Reference.ProfilesPath, _ProfileName);
            if (!String.IsNullOrEmpty(_ProfileName) && Profiles.GetProfile() != _ProfileName) {
                if (!Directory.Exists(_ProfilePath)) {
                    Directory.Move(Profiles.GetProfilePath(), _ProfilePath);
                } else {
                    Utils.Log(String.Format(Langs.Get("profile_already_exist"), _ProfileName));
                    _ProfileName = Profiles.GetProfile();
                }
            }

            // Refresh list
            Profiles.RefreshList(null);
            // Select profile
            Profiles.Select(_ProfileName);

            // Close
            this.Close();
        }
    }
}
