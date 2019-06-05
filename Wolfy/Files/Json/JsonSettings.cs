namespace Wolfy.Files.Json {
    public class JsonSettings {

        // General
        public string Language { get; set; }
        public bool Load_profile_startup { get; set; }
        public string Profile_startup { get; set; }
        public bool Check_for_updates { get; set; }
        public bool Launch_startup { get; set; }
        public bool Reduce_system_tray { get; set; }

        // Recognition
        public string Synthesizer_voice { get; set; }
        public string Speech_language { get; set; }
        public float Confidence { get; set; }
        public bool Recognition_at_launch { get; set; }

        // Material design
        public string Color { get; set; }
        public string Theme { get; set; }

    }
}
