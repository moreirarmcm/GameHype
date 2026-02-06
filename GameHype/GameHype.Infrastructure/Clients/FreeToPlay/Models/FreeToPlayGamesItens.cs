using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameHype.Infrastructure.Clients.FreeToPlay.Models
{
    public class FreeToPlayGamesItens
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        
        [JsonPropertyName("genre")]
        public string Genre { get; set; } = string.Empty;
        
        [JsonPropertyName("platform")]
        public string Platform { get; set; } = string.Empty;
        
        [JsonPropertyName("game_url")]
        public string Url { get; set; } = string.Empty;

    }
}
