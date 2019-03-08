using MoonSharp.Interpreter;
using System;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using Wolfy.Classes.Recognition;

namespace Wolfy.Classes {
    public static class Profiles {

        // ----------------| Variables |---------------- //
        private static ComboBox _ProfilesBox;
        private static Boolean IsReloading;

        /// <summary>
        /// Load the module managing the profiles
        /// </summary>
        public static void Init() {

            // Controls
            _ProfilesBox = Reference.MainWindow.ProfilesCombo;

            // Load profiles list
            RefreshList(false);

            // Event
            _ProfilesBox.SelectionChanged += delegate {
                if (!IsReloading) {
                    if (Exist(GetProfile())) {
                        Select(GetProfile());
                    } else {
                        RefreshList(false);
                    }
                }
            };

        }

        #region Profiles

        /// <summary>
        /// Checks if the given profile exists
        /// </summary>
        /// <param name="_ProfileName">Profile name</param>
        /// <returns>True or false</returns>
        public static Boolean Exist(String _ProfileName) {
            return (_ProfileName != null && Directory.Exists(Path.Combine(Reference.ProfilesPath, _ProfileName))) ? true : false;
        }

        /// <summary>
        /// Gives the access paths of the profiles
        /// </summary>
        /// <returns>Path</returns>
        public static String[] GetProfiles() {
            return Directory.GetDirectories(Reference.ProfilesPath, "*", SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        /// Gives the path of the selected profile
        /// </summary>
        /// <returns>Path</returns>
        public static String GetProfilePath() {
            String _ProfileName = GetProfile();
            return (_ProfileName != null) ? Path.Combine(Reference.ProfilesPath, _ProfileName) : null;
        }

        /// <summary>
        /// Gives the name of the selected profile
        /// </summary>
        /// <returns>Profile name</returns>
        public static String GetProfile() {
            return (_ProfilesBox.SelectedIndex > -1) ? _ProfilesBox.SelectedValue.ToString() : null;
        }

        /// <summary>
        /// Select the profile given by its name
        /// </summary>
        /// <param name="_ProfileName">Profile name</param>
        public static void Select(String _ProfileName) {
            // Prevent crash
            IsReloading = true;

            // Select
            _ProfilesBox.SelectedValue = (_ProfileName != null && Exist(_ProfileName)) ? _ProfileName : (GetProfiles().Count() > 0) ? ((ListBoxItem)_ProfilesBox.Items[0]).Content : null;

            // Prevent crash
            IsReloading = false;

            // Load
            SpeechRecognition.LoadCommands();
            SpeechRecognition.State = SpeechRecognition.State;

        }

        /// <summary>
        /// Refreshes the list of profiles
        /// _SelectSelectedProfile == null, select no profile
        /// </summary>
        /// <param name="_SelectSelectedProfile">Select the selected profile after the refresh</param>
        public static void RefreshList(Boolean? _SelectSelectedProfile) {

            // Prevent crash
            IsReloading = true;

            // Save selected profile
            String _SelectedProfile = (_SelectSelectedProfile != null && _SelectSelectedProfile.Value) ? GetProfile() : null;

            // Clear
            _ProfilesBox.Items.Clear();
            // Add profiles
            if (GetProfiles().Count() > 0) {
                GetProfiles().ToList().ForEach(a => _ProfilesBox.Items.Add(new ListBoxItem() { Content = Path.GetFileName(a) }));

                // Select profile
                if (_SelectSelectedProfile != null) {
                    if (_SelectSelectedProfile.Value && _SelectedProfile != null) {
                        Select(_SelectedProfile);
                    } else {
                        Select(null);
                    }
                }
            }

            // Prevent crash
            IsReloading = false;

        }

        #endregion

        #region Commands

        /// <summary>
        /// Gives the paths to the commands of the selected profile
        /// </summary>
        /// <returns>Path</returns>
        public static String[] GetProfileCommands() {
            String _ProfilePath = GetProfilePath();
            return (_ProfilePath != null) ? Directory.GetFiles(_ProfilePath, "*.lua", SearchOption.TopDirectoryOnly) : null;
        }

        /// <summary>
        /// Execute a command by name
        /// </summary>
        /// <param name="_CommandName">Command name</param>
        public static void ExecuteCommand(String _CommandName) {

            try {

                // Script
                Script _Command = new Script();

                // Init
                _Command.Options.DebugPrint = s => Utils.Log(s);

                // Variables
                _Command.Globals["command_name"] = _CommandName;

                // Execute
                DynValue _Script = _Command.LoadFile(Path.Combine(GetProfilePath(), _CommandName + ".lua"));
                _Script.Function.Call();

            } catch (Exception e) {
                Utils.Log(e.Message);
            }

        }

        #endregion

    }
}
