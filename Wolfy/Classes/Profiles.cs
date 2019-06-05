using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wolfy.Classes.Recognition;
using Wolfy.Files.Json;

namespace Wolfy.Classes {
    public static class Profiles {

        // |-------[ Variables ]-------| //
        private static ComboBox _ProfilesBox;
        private static Boolean IsReloading;

        private static ScriptEngine Engine;
        private static ScriptScope Scope;

        public static void Init() {

            #region Profiles

            // Controls
            _ProfilesBox = Reference.MainWindow.ProfilesCombo;

            // Select profile from settings
            if (Reference.JsonSettings.Load_profile_startup) {

                string ProfileStartup = Reference.JsonSettings.Profile_startup;

                // Profile is valid
                if (Exist(ProfileStartup)) {
                    RefreshList(null);
                    Select(ProfileStartup);
                } else {
                    Reference.JsonSettings.Profile_startup = null;
                    RefreshList(false);
                }

            }
            // Select first profile
            else {
                RefreshList(false);
            }

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

            #endregion

            // Init commands
            InitCommands();

        }

        #region Profiles

        /// <summary>
        /// Checks if the given profile exists
        /// </summary>
        public static Boolean Exist(String _ProfileName) {
            return (_ProfileName != null && Directory.Exists(Path.Combine(Reference.ProfilesPath, _ProfileName))) ? true : false;
        }

        /// <summary>
        /// Gives the access paths of the profiles
        /// </summary>
        public static String[] GetProfiles() {
            return Directory.GetDirectories(Reference.ProfilesPath, "*", SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        /// Gives the path of the selected profile
        /// </summary>
        public static String GetProfilePath() {
            String _ProfileName = GetProfile();
            return (_ProfileName != null) ? Path.Combine(Reference.ProfilesPath, _ProfileName) : null;
        }

        /// <summary>
        /// Gives the name of the selected profile
        /// </summary>
        public static String GetProfile() {
            return (_ProfilesBox.SelectedIndex > -1) ? _ProfilesBox.SelectedValue.ToString() : null;
        }

        /// <summary>
        /// Select the profile given by its name
        /// </summary>
        public static void Select(String _ProfileName) {
            // Prevent crash
            IsReloading = true;
            bool _RecognitionBackup = SpeechRecognition.State;
            SpeechRecognition.State = false;

            // Select
            _ProfilesBox.SelectedValue = (_ProfileName != null && Exist(_ProfileName)) ? _ProfileName : (GetProfiles().Count() > 0) ? ((ListBoxItem)_ProfilesBox.Items[0]).Content : null;

            // Prevent crash
            IsReloading = false;

            // Load
            SpeechRecognition.LoadCommands();
            SpeechRecognition.State = _RecognitionBackup;

        }

        /// <summary>
        /// Refreshes the list of profiles
        /// _SelectSelectedProfile == null, select no profile
        /// </summary>
        public static void RefreshList(Boolean? _SelectSelectedProfile) {

            // Prevent crash
            IsReloading = true;

            // Save selected profile
            String _SelectedProfile = (_SelectSelectedProfile != null && _SelectSelectedProfile.Value) ? GetProfile() : null;

            // Clear
            _ProfilesBox.Items.Clear();
            // Add profiles
            string[] _Profiles = GetProfiles();
            if (_Profiles.Count() > 0) {
                _Profiles.ToList().ForEach(a => _ProfilesBox.Items.Add(new ListBoxItem() { Content = Path.GetFileName(a) }));

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
        public static List<JsonCommand> GetProfileCommands() {
            String _ProfilePath = GetProfilePath();
            List<JsonCommand> _Commands = new List<JsonCommand>();

            if (!String.IsNullOrEmpty(_ProfilePath)) {

                // Retrieve commands
                foreach (String _CommandPath in Directory.GetDirectories(_ProfilePath, "*", SearchOption.TopDirectoryOnly)) {
                    _Commands.Add(GetCommand(Path.GetFileNameWithoutExtension(_CommandPath)));
                }

            }

            return _Commands;

        }

        /// <summary>
        /// Return a JsonCommand class by a command name
        /// Return null if the command isn't valid
        /// </summary>
        public static JsonCommand GetCommand(string _CommandName) {

            // Command is valid
            string _CommandPath = Path.Combine(GetProfilePath(), _CommandName);
            string _CommandInfo = Path.Combine(_CommandPath, "command.info");

            if (File.Exists(_CommandInfo)) {

                // Retrieve
                JsonCommand _JsonCommand = JsonConvert.DeserializeObject<JsonCommand>(File.ReadAllText(_CommandInfo));
                _JsonCommand.CommandName = _CommandName;
                _JsonCommand.CommandPath = _CommandPath;

                // Return
                return _JsonCommand;

            } else {
                return null;
            }

        }

        private static void InitCommands() {

            // Engine
            Engine = Python.CreateEngine();

            // Modules
            var _Paths = Engine.GetSearchPaths();
            _Paths.Add(Reference.ModulesPath);
            _Paths.Add(Reference.IronModules);
            Engine.SetSearchPaths(_Paths);

        }

        /// <summary>
        /// Execute a command by name
        /// </summary>
        public static void ExecuteCommand(string _CommandName) {

            // Start the command in another thread (prevent app from freezing)
            JsonCommand _Command = GetCommand(_CommandName);
            Task.Run(() => ExecCommand(_Command));

        }

        private static void ExecCommand(JsonCommand _Command) {

            try {

                // Variables
                Scope = Engine.CreateScope();
                foreach (KeyValuePair<string, string> _Pair in _Command.Command_vars) {
                    Scope.SetVariable(_Pair.Key, _Pair.Value);
                }

                // Errors : Messages
                var _Errors = new MemoryStream();
                var _Out = new MemoryStream();

                Engine.Runtime.IO.SetErrorOutput(_Errors, Encoding.UTF8);
                Engine.Runtime.IO.SetOutput(_Out, Encoding.UTF8);

                // Execute the script
                var _Source = Engine.CreateScriptSourceFromFile(Path.Combine(_Command.CommandPath, _Command.Command));
                _Source.Execute(Scope);

                // Show messages
                string ToReadable(byte[] a) => Encoding.UTF8.GetString(a);
                Application.Current.Dispatcher.Invoke((Action)delegate () {

                    // Get string
                    string _ReadableErrors = ToReadable(_Errors.ToArray()).Trim();
                    string _ReadableOut = ToReadable(_Out.ToArray()).Trim();

                    // Print
                    if (!string.IsNullOrEmpty(_ReadableErrors)) {
                        Utils.Log(_ReadableErrors);
                    }
                    if (!string.IsNullOrEmpty(_ReadableOut)) {
                        Utils.Log(_ReadableOut);
                    }

                });

            } catch (Exception e) {

                Application.Current.Dispatcher.Invoke((Action)delegate () {

                    // Handle some errors like encoding errors
                    Utils.Log(Engine.GetService<ExceptionOperations>().FormatException(e));

                });

            }
        }

        #endregion

    }
}
