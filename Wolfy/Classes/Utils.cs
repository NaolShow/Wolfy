using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace Wolfy.Classes {

    public static class Utils {

        #region General

        /// <summary>
        /// Useful functions
        /// </summary>
        public static class General {

            /// <summary>
            /// Restarts the application
            /// </summary>
            public static void RestartApplication() {

                // Start application
                Process.Start(Reference.AppFile);

                // Close application
                Environment.Exit(0);

            }

        }

        #endregion
        #region Json

        /// <summary>
        /// Useful functions for .json files
        /// </summary>
        public static class Json {

            /// <summary>
            /// Checks if json text is in a correct format
            /// </summary>
            public static bool IsValid(string json) {
                try {

                    JToken.Parse(json);
                    return true;

                } catch { return false; }
            }

            /// <summary>
            /// Copies the values of the current json into the updated one.
            /// Deletes the values of the current json if they don't exists in the updated one.
            /// </summary>
            public static string Merge(string current, string updated) {

                // Get json
                JObject currentObject = JObject.Parse(current);
                JObject updatedObject = JObject.Parse(updated);

                // Merge
                updatedObject.Merge(currentObject);

                return updatedObject.ToString();

            }

        }

        #endregion
        #region Files

        /// <summary>
        /// Useful functions for files
        /// </summary>
        public static class Files {

            /// <summary>
            /// Converts bytes to readable sting
            /// </summary>
            public static String BytesToString(long byteCount) {
                string[] units = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
                if (byteCount == 0)
                    return "0" + units[0];
                long bytes = Math.Abs(byteCount);
                int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
                double num = Math.Round(bytes / Math.Pow(1024, place), 1);
                return (Math.Sign(byteCount) * num).ToString() + units[place];
            }

            /// <summary>
            /// Get file checksum
            /// </summary>
            public static string GetFileChecksum(string file, HashAlgorithm algorithm) {
                using (FileStream fs = File.OpenRead(file)) {
                    return BitConverter.ToString(algorithm.ComputeHash(fs)).ToLower().Replace("-", "");
                }
            }

        }

        #endregion

    }

}
