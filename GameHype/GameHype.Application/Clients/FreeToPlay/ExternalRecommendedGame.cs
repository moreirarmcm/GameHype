using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHype.Application.Clients.FreeToPlay
{
    public class ExternalRecommendedGame
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Platform { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public int MinimumRamMb { get; set; }
    }
}
