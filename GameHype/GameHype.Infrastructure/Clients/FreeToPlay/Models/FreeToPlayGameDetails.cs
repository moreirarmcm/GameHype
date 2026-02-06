using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameHype.Infrastructure.Clients.FreeToPlay.Models
{
    public class FreeToPlayGameDetails
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("minimum_system_requirements")]
        public FreeToPlayMinimumSystemRequirements? MinimumSystemRequirements { get; set; }
    }

    public sealed class FreeToPlayMinimumSystemRequirements
    {
        [JsonPropertyName("memory")]
        public string? Memory { get; set; }
    }
}


