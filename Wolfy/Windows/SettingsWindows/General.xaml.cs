using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Wolfy.Classes;

namespace Wolfy.Windows.SettingsWindows {

    public partial class General : UserControl {

        public General() {
            InitializeComponent();

            #region Language

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

            #endregion
            #region Themes

            // List
            SkinManager.Themes.ForEach(a => ThemeCombo.Items.Add(new ListBoxItem() { Content = a.FirstLetterToUpperCase(), Tag = a }));
            // Value
            ThemeCombo.SelectedValue = Reference.JsonSettings.Theme;
            // Event
            ThemeCombo.SelectionChanged += delegate { SkinManager.SetTheme(ThemeCombo.SelectedValue.ToString()); };

            #endregion
            #region Colors

            // List
            SkinManager.Colors.ForEach(a => ColorCombo.Items.Add(new ListBoxItem() { Content = a.FirstLetterToUpperCase(), Tag = a }));
            // Value
            ColorCombo.SelectedValue = Reference.JsonSettings.Color;
            // Event
            ColorCombo.SelectionChanged += delegate { SkinManager.SetColor(ColorCombo.SelectedValue.ToString()); };

            #endregion

            #region Profile Startup

            // Value
            Profiles.GetProfiles().ToList().ForEach(a => ProfileStartupCombo.Items.Add(new ListBoxItem() { Content = Path.GetFileNameWithoutExtension(a) }));
            ProfileStartupCombo.SelectedValue = Reference.JsonSettings.Profile_startup;
            ProfileStartupCombo.IsEnabled = Reference.JsonSettings.Load_profile_startup;

            ProfileStartupCb.IsChecked = Reference.JsonSettings.Load_profile_startup;

            // Event
            ProfileStartupCombo.SelectionChanged += delegate {
                Reference.JsonSettings.Profile_startup = ProfileStartupCombo.SelectedValue.ToString();
            };

            #endregion

            #region Checkboxes

            LaunchStartupCb.IsChecked = Reference.JsonSettings.Launch_startup;
            CheckUpdateCb.IsChecked = Reference.JsonSettings.Check_for_updates;
            SystemTrayCb.IsChecked = Reference.JsonSettings.Reduce_system_tray;

            #endregion

        }

        #region Profile Startup

        // Profile at startup
        private void ProfileStartupCb_Checked(object sender, RoutedEventArgs e) {
            Reference.JsonSettings.Load_profile_startup = ProfileStartupCb.IsChecked.Value;

            // Enable/Disable combobox
            ProfileStartupCombo.IsEnabled = ProfileStartupCb.IsChecked.Value;
        }

        #endregion

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
