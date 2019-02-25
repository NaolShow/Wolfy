using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Wolfy.Files.Json;

namespace Wolfy.Classes {
    public static class Profiles {

        // ----------------| Variables |---------------- //
        private static JsonProfile CurrentProfile;

        /// <summary>
        /// Load the module managing the profiles
        /// </summary>
        public static void Init() {

            // Init ProfilesCombo
            foreach (String _Profile in Directory.GetFiles(Reference.ProfilesPath)) {
                ComboBoxItem _Item = new ComboBoxItem() {
                    Content = Path.GetFileNameWithoutExtension(_Profile)
                };
                // Add item to combobox
                Reference.MainWindow.ProfilesCombo.Items.Add(_Item);
            }

            // Init CurrentProfile var
            GetCurrentProfile();

        }

        /// <summary>
        /// Returns the profile that is currently selected
        /// </summary>
        /// <returns>JsonProfile</returns>
        public static JsonProfile GetCurrentProfile() {

            // If no profile exist
            if (Reference.MainWindow.ProfilesCombo.Items.Count == 0)
                return null;

            // Get selected profile name
            String _SelectedProfile = Reference.MainWindow.ProfilesCombo.SelectedValue.ToString();

            // If profile doesn't exist
            if (!File.Exists(Reference.ProfilesPath + _SelectedProfile + ".json"))
                return null;

            // Load the profile
            if (CurrentProfile.Name != _SelectedProfile)
                CurrentProfile = JsonConvert.DeserializeObject<JsonProfile>(File.ReadAllText(Reference.ProfilesPath + _SelectedProfile + ".json"));
            // Return
            return CurrentProfile;

        }

    }
}
