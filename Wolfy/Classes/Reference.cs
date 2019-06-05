using System;
using System.Collections.Generic;
using System.Windows;
using Wolfy.Files.Json;
using Wolfy.Windows;

namespace Wolfy.Classes {
    public static class Reference {

        /** ----------------| Useful Variables |---------------- **/
        public static readonly string AppName = Application.ResourceAssembly.GetName().Name;
        public static readonly string AppVersion = "1.0.0";
        public static readonly string AppPath = System.AppDomain.CurrentDomain.BaseDirectory;

        /** ----------------| Windows Variables |---------------- **/
        public static Main MainWindow;

        /** ----------------| Folders Variables |---------------- **/
        public static readonly string UpdateLink = @"https://dl.dropbox.com/s/09rmbalpk1exlr9/update.txt";
        public static readonly string TempUpdateFile = TempUpdatePath + "update.exe";
        // Update files
        public static readonly string TempUpdatePath = AppPath + @"update.temp\";
        public static readonly string TempUpdateZip = AppPath + @"update.temp.zip";

        /** ----------------| Folders Variables |---------------- **/
        public static readonly string SettingsPath = AppPath + @"Settings\";
        public static readonly string LangsPath = AppPath + @"Langs\";
        public static readonly string ProfilesPath = AppPath + @"Profiles\";
        public static readonly string ModulesPath = AppPath + @"Modules\";
        public static readonly string[] Folders = new string[] {
            SettingsPath,
            LangsPath,
            ProfilesPath,
            ModulesPath
        };

        /** ----------------| Files Variables |---------------- **/
        public static readonly String Settings = AppPath + @"Settings\settings.json";
        public static readonly String IronModules = ModulesPath + @"IronPythonLib.zip";

        // These files are overwritten when the software is launched 
        public static readonly Dictionary<String, String> Files = new Dictionary<String, String> {
        };

        /** ----------------| Json Variables |---------------- **/
        public static JsonSettings JsonSettings;

    }
}
