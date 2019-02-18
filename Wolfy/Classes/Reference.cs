using Wolfy.Files.Json;
using Wolfy.Properties;
using Wolfy.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        public static readonly String[] Folders = new String[] {
            SettingsPath,
            LangsPath
        };

        /** ----------------| Files Variables |---------------- **/
        public static readonly String Settings = AppPath + @"Settings\settings.json";
        public static readonly Dictionary<String, String> Files = new Dictionary<String, String> {
            { Reference.LangsPath + "fr.json", Resources.fr },
            { Reference.LangsPath + "en.json", Resources.en },
            { Reference.LangsPath + "de.json", Resources.de },
            { Reference.LangsPath + "it.json", Resources.it },
            { Reference.LangsPath + "es.json", Resources.es },
            { Reference.LangsPath + "ru.json", Resources.ru }
        };

        /** ----------------| Json Variables |---------------- **/
        public static JsonSettings JsonSettings;

    }
}
