using System.Windows;
using System.Windows.Controls;
using Wolfy.Classes;

namespace Wolfy.Windows.SettingsWindows {

    public partial class General : UserControl {

        public General() {
            InitializeComponent();

            // ----------------| Language |---------------- //

            // List
            Langs.GetLanguages().ForEach(a => LangCombo.Items.Add(new ListBoxItem() { Content = a["language_display_name"], Tag = a["language_name"] }));
            // Value
            LangCombo.SelectedValue = Reference.JsonSettings.Language;
            // Event
            LangCombo.SelectionChanged += delegate {
                Langs.SetLanguage(LangCombo.SelectedValue.ToString());
                Reference.JsonSettings.Language = LangCombo.SelectedValue.ToString();
            };

            // Translation error button
            TranslationErrorBtn.Click += delegate { System.Diagnostics.Process.Start("https://github.com/NaolShow/Wolfy/issues"); };

            // ----------------| Theme |---------------- //

            // List
            SkinManager.Themes.ForEach(a => ThemeCombo.Items.Add(new ListBoxItem() { Content = a.FirstLetterToUpperCase(), Tag = a }));
            // Value
            ThemeCombo.SelectedValue = Reference.JsonSettings.Theme;
            // Event
            ThemeCombo.SelectionChanged += delegate { SkinManager.SetTheme(ThemeCombo.SelectedValue.ToString()); };

            // ----------------| Color |---------------- //

            // List
            SkinManager.Colors.ForEach(a => ColorCombo.Items.Add(new ListBoxItem() { Content = a.FirstLetterToUpperCase(), Tag = a }));
            // Value
            ColorCombo.SelectedValue = Reference.JsonSettings.Color;
            // Event
            ColorCombo.SelectionChanged += delegate { SkinManager.SetColor(ColorCombo.SelectedValue.ToString()); };

            // ----------------| Checkboxes |---------------- //

            LaunchStartupCb.IsChecked = Reference.JsonSettings.Launch_startup;
            CheckUpdateCb.IsChecked = Reference.JsonSettings.Check_for_updates;
            SystemTrayCb.IsChecked = Reference.JsonSettings.Reduce_system_tray;

        }

        #region Checkboxes

        // Launch at startup
        private void LaunchStartupCb_Checked(object sender, RoutedEventArgs e) {
            Reference.JsonSettings.Launch_startup = LaunchStartupCb.IsChecked.Value;
        }

        // Check for updates
        private void CheckUpdateCb_Checked(object sender, RoutedEventArgs e) {
            Reference.JsonSettings.Check_for_updates = CheckUpdateCb.IsChecked.Value;
        }

        // System tray at startup
        private void SystemTrayCb_Checked(object sender, RoutedEventArgs e) {
            Reference.JsonSettings.Reduce_system_tray = SystemTrayCb.IsChecked.Value;
        }

        #endregion

    }

}
