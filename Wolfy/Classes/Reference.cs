using System;
using System.Collections.Generic;
using System.Windows;
using Wolfy.Files.Json;
using Wolfy.Windows;

namespace Wolfy.Classes {
    public static class Reference {

        /** ----------------| Useful Variables |---------------- **/
        public static readonly String AppName = Application.ResourceAssembly.GetName().Name;
        public static readonly String AppVersion = "1.0.0";
        public static readonly String AppPath = System.AppDomain.CurrentDomain.BaseDirectory;

        /** ----------------| Windows Variables |---------------- **/
        public static Main MainWindow;

        /** ----------------| Folders Variables |---------------- **/
        public static readonly String UpdateLink = @"https://dl.dropbox.com/s/09rmbalpk1exlr9/update.txt";
        public static readonly String TempUpdateFile = TempUpdatePath + "update.exe";
        // Update files
        public static readonly String TempUpdatePath = AppPath + @"update.temp\";
        public static readonly String TempUpdateZip = AppPath + @"update.temp.zip";

        /** ----------------| Folders Variables |---------------- **/
        public static readonly String SettingsPath = AppPath + @"Settings\";
        public static readonly String LangsPath = AppPath + @"Langs\";
        public static readonly String ProfilesPath = AppPath + @"Profiles\";
        public static readonly String[] Folders = new String[] {
            SettingsPath,
            LangsPath,
            ProfilesPath
        };

        /** ----------------| Files Variables |---------------- **/
        public static readonly String Settings = AppPath + @"Settings\settings.json";

        // These files are overwritten when the software is launched 
        public static readonly Dictionary<String, String> Files = new Dictionary<String, String> {
        };

        /** ----------------| Json Variables |---------------- **/
        public static JsonSettings JsonSettings;

    }
}
