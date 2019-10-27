using System;
using System.IO;
using System.Reflection;

namespace Wolfy.Classes {

    public static class Reference {

        // ---- General

        private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();

        public static bool IsPortable;

        public static readonly string AppName = Assembly.GetName().Name;
        public static readonly Version AppVersion = Assembly.GetName().Version;

        public static readonly string AppFile = Assembly.Location;
        public static readonly string AppPath = Path.GetDirectoryName(AppFile);
        public static readonly string AppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName);

        // ---- Folders

        public static class Folders {

            public static string Langs { get => Path.Combine((IsPortable) ? AppPath : AppData, "Langs"); }

        }

        // ---- Files

        public static class Files {

            public static string Update { get => Path.Combine((IsPortable) ? AppPath : AppData, "Update.rar"); }
            public static string Settings { get => Path.Combine((IsPortable) ? AppPath : AppData, "Settings.json"); }

        }

    }

}
