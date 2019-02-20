using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolfy.Files.Json {
    public class JsonSettings {

        // General
        public String Language { get; set; }
        public Boolean Check_for_updates { get; set; }

        // Recognition
        public String Synthesizer_voice { get; set; }
        public String Speech_language { get; set; }
        [DefaultValue(80)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public float Confidence { get; set; }

        // Material design
        public String Color { get; set; }
        public String Theme { get; set; }

    }
}
