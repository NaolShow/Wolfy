using System;
using System.Collections.Generic;
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

namespace Wolfy.Windows {

    public partial class SettingsWindow : Window, ITranslation {

        private void EmbedUserControl(UserControl _UserControl) {
            // Embed
            Grid.Children.Clear();
            Grid.Children.Add(_UserControl);

            // Translate
            Translation.TranslateWindow(this);
        }

        public SettingsWindow() {
            InitializeComponent();

            GeneralBtn_Click(null, new RoutedEventArgs());
        }

        // ----------------| UserControls |---------------- //
        SettingsWindows.General General;
        SettingsWindows.Recognition Recognition;

        public void OnTranslation() {

            // Translation

        }

        #region Menu

        private void GeneralBtn_Click(object sender, RoutedEventArgs e) {
            // Show general tab
            if (General == null)
                General = new SettingsWindows.General();
            EmbedUserControl(General);
        }

        private void RecognitionBtn_Click(object sender, RoutedEventArgs e) {
            // Show recognition tab
            if (Recognition == null)
                Recognition = new SettingsWindows.Recognition();
            EmbedUserControl(Recognition);
        }

        #endregion

        // Close settings window
        private void OkBtn_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
