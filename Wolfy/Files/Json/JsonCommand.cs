using Newtonsoft.Json;
using System.Collections.Generic;

namespace Wolfy.Files.Json {
    public class JsonCommand {

        [JsonIgnore]
        public string CommandName { get; set; }
        [JsonIgnore]
        public string CommandPath { get; set; }
        public string Command { get; set; }
        public Dictionary<string, string> Command_vars { get; set; }

    }
}
