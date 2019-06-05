using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Wolfy.Classes {
    public static class Utils {

        #region General

        /// <summary>
        /// Simply close the application
        /// </summary>
        public static void CloseApplication() {

            // Save json files
            Dictionary<String, Object> SavedJson = new Dictionary<String, Object>() {
                { Reference.Settings, Reference.JsonSettings }
            };
            foreach (KeyValuePair<String, Object> entry in SavedJson) {
                File.WriteAllText(entry.Key, JsonConvert.SerializeObject(entry.Value, Formatting.Indented));
            }

            // Exit
            Environment.Exit(0);

        }

        /// <summary>
        /// Simply restart the application
        /// </summary>
        public static void RestartApplication() {

            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            CloseApplication();

        }

        internal static void Log(int v) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the input string with the first character converted to uppercase
        /// </summary>
        public static string FirstLetterToUpperCase(this string _s) {
            if (string.IsNullOrEmpty(_s))
                return string.Empty;

            char[] _a = _s.ToCharArray();
            _a[0] = char.ToUpper(_a[0]);
            return new string(_a);
        }

        #endregion

        #region Windows

        /// <summary>
        /// Embed a user control in a grid
        /// </summary>
        public static void EmbedUserControl(UserControl _UserControl, Grid _Grid) {
            // Embed
            _Grid.Children.Clear();
            _Grid.Children.Add(_UserControl);
        }

        #endregion

        #region Json

        /// <summary>
        /// Checks if the Json file is correct
        /// </summary>
        public static bool IsValidJson(String _Json) {
            try {
                JToken.Parse(_Json);
                return true;
            } catch {
                return false;
            }
        }

        /// <summary>
        /// Copies the values of the file _Current to the file _Updated 
        /// Deletes the values if they do not exist in the file _Updated) 
        /// Then returns a String (being the result)
        /// </summary>
        public static String MergeJson(String _Current, String _Updated) {

            // Retrieve both json
            JObject _CurrentObject = JObject.Parse(_Current, new JsonLoadSettings() { CommentHandling = CommentHandling.Load });
            JObject _UpdatedObject = JObject.Parse(_Updated, new JsonLoadSettings() { CommentHandling = CommentHandling.Load });

            // Merge
            _UpdatedObject.Merge(_CurrentObject);

            // Result
            return _UpdatedObject.ToString();

        }

        #endregion

        #region Files

        /// <summary>
        /// Remove special characters
        /// </summary>
        public static String RemoveSpecialCharacters(String _String) {
            return Regex.Replace(_String, "[^a-zA-Z0-9_]+", " ", RegexOptions.Compiled);
        }

        public static String BytesToString(long byteCount) {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }

        /// <summary>
        /// Returns the name of a valid file/folder (which does not exist)
        /// </summary>
        public static String GetValidFileID(Boolean _IsFolder, String _Path, String _BaseName, int _StartID = 0, String _Extension = null) {

            while (true) {

                // Check if folder/file doesn't exist
                if ((_IsFolder && !Directory.Exists(Path.Combine(_Path, _BaseName + _StartID))) || (!_IsFolder && !File.Exists(Path.Combine(_Path, _BaseName + _StartID + _Extension)))) {
                    return Path.Combine(_Path, _BaseName + _StartID + _Extension);
                }

                _StartID++;

            }

        }

        #endregion

        #region Logs

        public static void Log(String _Text) {

            // Add new line
            if (Reference.MainWindow.LogsTxt.Text != "")
                Reference.MainWindow.LogsTxt.AppendText(Environment.NewLine);

            // Add text
            Reference.MainWindow.LogsTxt.AppendText(_Text);

        }

        /// <summary>
        /// Shows an error by a label
        /// </summary>
        public static async void ErrorLabel(Label _Label, string _ErrorText, int _WaitInSecond = 0) {

            // Show error
            _Label.Content = _ErrorText;
            _Label.Visibility = Visibility.Visible;

            // Timer
            if (_WaitInSecond != 0) {
                await Task.Delay(_WaitInSecond * 1000);

                // Hide
                _Label.Visibility = Visibility.Hidden;
                _Label.Content = null;
            }

        }

        #endregion

    }
}
