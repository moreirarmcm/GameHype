using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHype.Infrastructure.Clients.FreeToPlay.Caching
{
    public class FreeToPlayCacheParams
    {
        public TimeSpan FilterTtl { get; init; } = TimeSpan.FromMinutes(10);
        public TimeSpan GameDetailsTtl { get; init; } = TimeSpan.FromMinutes(60);
    }
}
