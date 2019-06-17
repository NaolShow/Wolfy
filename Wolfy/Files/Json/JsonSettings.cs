namespace Wolfy.Files.Json {
    public class JsonSettings {

        // General
        public string Language { get; set; }
        public bool Load_profile_startup { get; set; } = false;
        public string Profile_startup { get; set; }
        public bool Check_for_updates { get; set; } = true;
        public bool Launch_startup { get; set; } = false;
        public bool Reduce_system_tray { get; set; } = false;

        // Material design
        public string Theme { get; set; } = "light";
        public string Color { get; set; } = "blue";

        // Recognition
        public string Speech_language { get; set; }
        public float Confidence { get; set; } = 80;
        public bool Recognition_at_launch { get; set; } = true;

        public string Synthesizer_voice { get; set; }

    }
}
