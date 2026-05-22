using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace UniversalHelmod.Extractors.StarRupture.Models
{
    public class FileSave
    {
        [JsonIgnore]
        public string Filename;

        [JsonIgnore]
        public string SaveFilename;

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }

        [JsonPropertyName("worldTimeSeconds")]
        public double WorldTimeSeconds { get; set; }

        [JsonPropertyName("bIsInTutorial")]
        public bool IsInTutorial { get; set; }

        [JsonPropertyName("gameVersion")]
        public GameVersion GameVersion { get; set; }
    }

    public class GameVersion
    {
        [JsonPropertyName("major")]
        public int Major { get; set; }

        [JsonPropertyName("minor")]
        public int Minor { get; set; }

        [JsonPropertyName("pATCH")]
        public int Patch { get; set; }
    }
}
