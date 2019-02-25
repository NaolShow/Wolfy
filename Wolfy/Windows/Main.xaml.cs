using Wolfy.Classes;
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

namespace Wolfy.Windows {
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window, ITranslation {
        public Main() {
            InitializeComponent();
        }

        // Translation event
        public void OnTranslation() {
            Console.WriteLine("Translating main form...");
        }

        // Needed to close the app process
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            Utils.CloseApplication();
        }
        
        // Auto scroll
        private void LogsTxt_TextChanged(object sender, TextChangedEventArgs e) {
            LogsTxt.ScrollToEnd();
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e) {

            // Show settings window
            SettingsWindow _settings = new SettingsWindow();
            Translation.TranslateWindow(_settings);
            _settings.ShowDialog();

        }
    }
}
