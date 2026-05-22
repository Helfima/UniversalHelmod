using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace UniversalHelmod.Extractors.StarRupture.Models
{
    public class DataSave
    {
        [JsonPropertyName("itemData")]
        public SRItemData ItemData;

        [JsonPropertyName("level")]
        public string Level;

        [JsonPropertyName("worldTimeSeconds")]
        public double WorldTimeSeconds;

        [JsonPropertyName("worldUnpausedTimeSeconds")]
        public double WorldUnpausedTimeSeconds;

        [JsonPropertyName("worldRealTimeSeconds")]
        public double WorldRealTimeSeconds;

        [JsonPropertyName("worldAudioTimeSeconds")]
        public double WorldAudioTimeSeconds;

        [JsonPropertyName("sessionName")]
        public string SessionName;

        [JsonPropertyName("saveName")]
        public string SaveName;

        [JsonPropertyName("timestamp")]
        public string Timestamp;

        [JsonPropertyName("gameVersion")]
        public GameVersion GameVersion;

    }
    public class SRItemData
    {
        [JsonPropertyName("Mass")]
        public SRMass Mass;
    }
    public class SRMass
    {
        [JsonPropertyName("entities")]
        public SREntities Entities;
    }
    public class SREntities : Dictionary<string, SREntity>
    {
    }

    public class SREntity
    {
        [JsonPropertyName("spawnData")]
        public SRSpawnData SpawnData;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; }

        [JsonPropertyName("fragmentValues")]
        public List<string> FragmentValues { get; set; }

        [JsonIgnore]
        public bool IsProducer { get; set; }
        [JsonIgnore]
        public string RecipePath { get; set; }
        [JsonIgnore]
        public List<SRTranslation> Link { get; set; } = new List<SRTranslation>();
    }

    public class SRSpawnData
    {
        [JsonPropertyName("entityConfigDataPath")]
        public string EntityConfigDataPath { get; set; }

        [JsonPropertyName("transform")]
        public SRTransform Transform { get; set; }

    }
    public class SRTransform
    {
        [JsonPropertyName("rotation")]
        public SRRotation Rotation;

        [JsonPropertyName("translation")]
        public SRTranslation Translation;

        [JsonPropertyName("scale3D")]
        public SRScale3D Scale3D;
    }
    public class SRRotation
    {
        [JsonPropertyName("x")]
        public double X;

        [JsonPropertyName("y")]
        public double Y;

        [JsonPropertyName("z")]
        public double Z;

        [JsonPropertyName("w")]
        public double W;
    }

    public class SRTranslation
    {
        [JsonPropertyName("x")]
        public double X;

        [JsonPropertyName("y")]
        public double Y;

        [JsonPropertyName("z")]
        public double Z;
    }
    public class SRScale3D
    {
        [JsonPropertyName("x")]
        public double X;

        [JsonPropertyName("y")]
        public double Y;

        [JsonPropertyName("z")]
        public double Z;
    }

 }
