using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
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
        /// <param name="_UserControl">User control</param>
        /// <param name="_Grid">Grid</param>
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
        /// <param name="_Json">Json file as String</param>
        /// <returns></returns>
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
        /// <param name="_Current">Current Json file</param>
        /// <param name="_Updated">Updated Json file</param>
        /// <returns></returns>
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
        /// <param name="_String">String</param>
        /// <returns>Removed special characters string</returns>
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
        /// <param name="_IsFolder">Is folder?</param>
        /// <param name="_Path">Path</param>
        /// <param name="_BaseName">Base file/folder name</param>
        /// <param name="_StartID">Start id</param>
        /// <param name="_Extension">Extension just for files</param>
        /// <returns></returns>
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

        #endregion

    }
}
