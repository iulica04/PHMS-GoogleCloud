using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.Enums;

namespace Domain.Entities
{
    public class ExternalMedication
    {
        public required string Name { get; set; }

        [JsonIgnore]
        public MedicationType Type { get; private set; }

        [JsonPropertyName("type")]
        public string TypeString
        {
            get => Type.ToString();
            set => Type = Enum.TryParse<MedicationType>(value, true, out var parsedType) ? parsedType : throw new JsonException($"Invalid MedicationType: {value}");
        }

        public required string Dosage { get; set; }
        public required string Administration { get; set; }
        public required string Ingredients { get; set; }

        [JsonPropertyName("adverse_effects")]
        public string AdverseEffects { get; set; }
    }
}
