using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolfy.Files.Json {
    public class JsonLang {

        // Language name
        public String Name { get; set; }
        // Language description
        public String Description { get; set; }
        // Language version
        public String Version { get; set; }

        // Translations
        public Dictionary<String, String> Translations { get; set; }

    }
}
