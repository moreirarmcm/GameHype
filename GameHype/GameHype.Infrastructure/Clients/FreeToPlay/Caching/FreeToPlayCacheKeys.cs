using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHype.Infrastructure.Clients.FreeToPlay.Caching
{
    public class FreeToPlayCacheKeys
    {
        public static string FilterCacheKey(List<string> genre, string? platform)
        {
            var platformNormalized = NormalizedPlatform(platform);  
            var genreNormalized = NormalizedGenre(genre);

            return $"f2p:filter:{platformNormalized}:{genreNormalized}";
        }

        public static string GameDetailsCacheKey(int gameId)
        {
            return $"f2p:game:{gameId}";
        }

        public static string NormalizedPlatform(string? platform)
        {
            return string.IsNullOrWhiteSpace(platform) ? "all" : platform.Trim().ToLower();
        }
        public static string NormalizedGenre(List<string> genre)
        {
            return string.Join('.', genre
                    .Where(g => !string.IsNullOrWhiteSpace(g))
                    .Select(g => g.Trim().ToLowerInvariant())
                    .Distinct()
                    .OrderBy(g => g));
        }
    }
}