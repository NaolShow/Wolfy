using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Wolfy.Classes;
using Wolfy.Files.Json;

namespace Wolfy.Windows.ProfilesWindows {

    public partial class ProfileWindow : Window
    {

        #region Filter commands (search)

        // Commands
        private List<ListBoxItem> CommandsList;

        private bool CustomFilter(object obj) {
            return (String.IsNullOrEmpty(SearchFilter.Text)) ? true : (obj.ToString().IndexOf(SearchFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private CollectionView View;
        private void SearchFilter_TextChanged(object sender, TextChangedEventArgs e) {
            CollectionViewSource.GetDefaultView(CommandsBox.ItemsSource).Refresh();
        }

        #endregion

        public ProfileWindow(JsonProfile _Profile)
        {
            InitializeComponent();

            // Window initialization
            // this.Title = String.Format(Translation.Get("profile_window_title"), _Profile.Name, "?");
            // Temp:
            this.Title = String.Format(Translation.Get("profile_window_title"), "Temporary profile", "?");

            String[] _FakeCommands = {
                "My command",
                "My other command",
                "This is for test",
                "Please star Wolfy <3",
                "Thanks :)",
                "I don't know what I could write.",
                "Super long command because this command is long okay ?"
            };

            // Add all commands to list
            CommandsList = new List<ListBoxItem>();
            foreach (String _Command in _FakeCommands) {

                // Create item
                ListBoxItem _Item = new ListBoxItem {
                    Content = _Command
                };

                // Add item
                CommandsList.Add(_Item);

            }

            // Search textbox
            this.DataContext = this;
            CommandsBox.ItemsSource = CommandsList;
            View = (CollectionView)CollectionViewSource.GetDefaultView(CommandsBox.ItemsSource);
            View.Filter = CustomFilter;

        }


        // Close window
        private void OkBtn_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

    }
}
