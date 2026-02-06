using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHype.Domain.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public int FreeToPlayId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public DateTime RecommendedAt { get; set; }
    }
}
 
