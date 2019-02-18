using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolfy.Files.Json {
    public class JsonSettings {

        // General
        public String Language { get; set; }
        public Boolean Check_for_updates { get; set; }

        // Material design
        public String Color { get; set; }
        public String Theme { get; set; }

    }
}
