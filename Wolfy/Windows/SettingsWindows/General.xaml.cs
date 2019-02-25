using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wolfy.Classes;
using Wolfy.Files.Json;

namespace Wolfy.Windows.SettingsWindows
{

    public partial class General : UserControl
    {

        public General()
        {
            InitializeComponent();

            // ----------------| Language |---------------- //
            InitLang();

            // ----------------| Theme |---------------- //
            ThemeCombo.Items.Add(new ComboBoxItem() { Content = "Dark", Tag = "dark" });
            ThemeCombo.Items.Add(new ComboBoxItem() { Content = "Light", Tag = "light" });
            ThemeCombo.Text = Reference.JsonSettings.Theme.ToLower().FirstLetterToUpperCase();

            // ----------------| Color |---------------- //
            foreach (String _Color in SkinManager.Colors) {
                ColorCombo.Items.Add(new ComboBoxItem() { Content = _Color.FirstLetterToUpperCase(), Tag = _Color });
            }
            ColorCombo.Text = Reference.JsonSettings.Color.ToLower().FirstLetterToUpperCase();

            // ----------------| Checkboxes |---------------- //
            LaunchStartupCb.IsChecked = Reference.JsonSettings.Launch_startup;
            CheckUpdateCb.IsChecked = Reference.JsonSettings.Check_for_updates;
            SystemTrayCb.IsChecked = Reference.JsonSettings.Reduce_system_tray;

        }

        #region Language

        private void InitLang() {
            foreach (String _Lang in Directory.GetFiles(Reference.LangsPath, "*.json", SearchOption.TopDirectoryOnly)) {
                // Retrieve informations
                JsonLang _JsonLang = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonLang>(File.ReadAllText(_Lang));

                // Add to box
                ComboBoxItem _Item = new ComboBoxItem() {
                    Content = _JsonLang.Name,
                    Tag = System.IO.Path.GetFileNameWithoutExtension(_Lang)
                };
                LangCombo.Items.Add(_Item);

            }
            LangCombo.Text = Translation.CurrentLang.Name;
        }

        // Change language
        private void LangCombo_SelectionChanged(object sender, SelectionChangedEventArgs e) {

            // Retrieve item tag
            String _Tag = LangCombo.SelectedValue.ToString();

            // Change lang
            Reference.JsonSettings.Language = _Tag;
            Translation.LoadLang();
            Translation.Translate();

        }

        #endregion

        #region Theme

        private void ThemeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e) {

            // Change theme
            Reference.JsonSettings.Theme = ThemeCombo.SelectedValue.ToString();
            SkinManager.ApplyTheme(Reference.JsonSettings.Theme);

        }

        #endregion

        #region Color

        private void ColorCombo_SelectionChanged(object sender, SelectionChangedEventArgs e) {

            // Change color
            Reference.JsonSettings.Color = ColorCombo.SelectedValue.ToString();
            SkinManager.ApplyColor(Reference.JsonSettings.Color);

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
