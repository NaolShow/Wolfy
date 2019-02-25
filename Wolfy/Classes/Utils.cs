using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

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
        /// Gets all the controls of a window, recursively
        /// </summary>
        /// <param name="_Window">Window</param>
        /// <returns></returns>
        public static List<Control> GetControlsOf(Window _Window) {
            return GetLogicalChildCollection<Control>(_Window);
        }

        // ---------| From https://stackoverflow.com/questions/14875042/finding-all-child-controls-wpf |--------- //

        /// <summary>
        /// Gets all the controls (of a type) of a window, recursively
        /// </summary>
        /// <param name="_Window">Window</param>
        /// <returns></returns>
        public static List<T> GetLogicalChildCollection<T>(object parent) where T : DependencyObject {
            List<T> logicalCollection = new List<T>();
            GetLogicalChildCollection(parent as DependencyObject, logicalCollection);
            return logicalCollection;
        }

        private static void GetLogicalChildCollection<T>(DependencyObject parent, List<T> logicalCollection) where T : DependencyObject {
            System.Collections.IEnumerable children = LogicalTreeHelper.GetChildren(parent);
            foreach (object child in children) {
                if (child is DependencyObject) {
                    DependencyObject depChild = child as DependencyObject;
                    if (child is T) {
                        logicalCollection.Add(child as T);
                    }
                    GetLogicalChildCollection(depChild, logicalCollection);
                }
            }
        }

        // ---------| From https://stackoverflow.com/questions/14875042/finding-all-child-controls-wpf |--------- //

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
            }
            catch {
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

        public static String BytesToString(long byteCount) {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }

        #endregion

        #region Logs

        public static void Log(String Text) {

            // Add new line
            if (Reference.MainWindow.LogsTxt.Text != "")
                Reference.MainWindow.LogsTxt.AppendText(Environment.NewLine);

            // Add text
            Reference.MainWindow.LogsTxt.AppendText(Text);

        }

        #endregion

    }
}
