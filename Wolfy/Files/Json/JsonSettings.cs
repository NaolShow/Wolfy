using System;

namespace Wolfy.Files.Json {
    public class JsonSettings {

        // General
        public String Language { get; set; }
        public Boolean Check_for_updates { get; set; }
        public Boolean Launch_startup { get; set; }
        public Boolean Reduce_system_tray { get; set; }

        // Recognition
        public String Synthesizer_voice { get; set; }
        public String Speech_language { get; set; }
        public float Confidence { get; set; }
        public Boolean Recognition_at_launch { get; set; }

        // Material design
        public String Color { get; set; }
        public String Theme { get; set; }

    }
}
