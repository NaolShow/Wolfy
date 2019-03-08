using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Wolfy.Classes;

namespace Wolfy.Windows {

    public partial class SettingsWindow : Window {

        public SettingsWindow() {
            InitializeComponent();

            // ----------------| Menu |---------------- //
            Dictionary<String, UserControl> _Menus = new Dictionary<String, UserControl>() {
                { "general", new SettingsWindows.General() },
                { "recognition", new SettingsWindows.Recognition() }
            };

            foreach (KeyValuePair<String, UserControl> _Pair in _Menus) {
                // Item
                ListBoxItem _Item = new ListBoxItem();
                _Item.SetResourceReference(ListBoxItem.ContentProperty, _Pair.Key);
                // Event
                _Item.Selected += delegate { Utils.EmbedUserControl(_Pair.Value, Grid); };
                // Add
                Menu.Items.Add(_Item);
            }

            // Embed first of list
            Utils.EmbedUserControl(_Menus.Values.First(), Grid);
        }

        // Close settings window
        private void OkBtn_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
