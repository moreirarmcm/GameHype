
using GameHype.Application.Clients.FreeToPlay;
using GameHype.Application.Clients.FreeToPlay.Interfaces;
using GameHype.Application.DTOs;
using GameHype.Infrastructure.Clients.FreeToPlay.Caching;
using GameHype.Infrastructure.Clients.FreeToPlay.Casting;
using GameHype.Infrastructure.Clients.FreeToPlay.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace GameHype.Infrastructure.Clients.FreeToPlay
{
    public class FreeToPlayClient : IFreeToPlayClient
    {
        private readonly HttpClient _http;
        private readonly IMemoryCache _memoryCache;
        private readonly FreeToPlayCacheParams _cacheParams;

        public FreeToPlayClient(HttpClient http, IMemoryCache cache, FreeToPlayCacheParams cacheParams)
        {
            _http = http;
            _memoryCache = cache;
            _cacheParams = cacheParams;
        }
        public async Task <ExternalRecommendedGame?> GetRecommendedGameAsync(List<string> genre, string? platform, int? ramMb)
        {
            var gamesCandidates = await GetGamesCandidatesInCacheAsync(genre, platform); 
            if (gamesCandidates.Count == 0) return null;

            if (ramMb is null)
            {
                var chosenGame = ChooseRecommendedGame(gamesCandidates);
                return MapToExternalRecommendedGame(chosenGame);
            }
            var gamesCandidatesSuffled = gamesCandidates.OrderBy(g => Random.Shared.Next()).ToList();

            foreach(var game in gamesCandidatesSuffled)
            {
                var gameDetails = await GetGameById(game.Id);
                if (gameDetails.MinimumRamMb is null) continue;

                if (gameDetails.MinimumRamMb <= ramMb)
                {
                    return MapToExternalRecommendedGame(game);
                }
            }
            return null;
        }
        public async Task<RecommendedGamesDetailsResponse> GetGameById(int id)
        {
            var gameKey = FreeToPlayCacheKeys.GameDetailsCacheKey(id);

            return await _memoryCache.GetOrCreateAsync(gameKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _cacheParams.GameDetailsTtl;
                var url = $"game?id={id}";
                var gameDetails = await _http.GetFromJsonAsync<FreeToPlayGameDetails>(url);
                
                if (gameDetails is null)
                {
                    return new RecommendedGamesDetailsResponse { Id = id, MinimumRamMb = null };
                }
                var minimumRamMb = RamConversor.ConvertRamStringToMb(gameDetails.MinimumSystemRequirements?.Memory);

                return new RecommendedGamesDetailsResponse {
                    Id = gameDetails.Id,
                    MinimumRamMb = minimumRamMb
                };
            }) ?? new RecommendedGamesDetailsResponse { Id = id, MinimumRamMb = null };
        }

        private async Task <List<FreeToPlayGamesItens>> GetGamesCandidatesInCacheAsync (List<string> genre, string? platform)
        {
            var cacheKey =  FreeToPlayCacheKeys.FilterCacheKey(genre, platform);

            return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _cacheParams.FilterTtl;            
                var tag = FreeToPlayCacheKeys.NormalizedGenres(genre);
                var platformNormalized = FreeToPlayCacheKeys.NormalizedPlatform(platform);
                if (string.IsNullOrWhiteSpace(tag)) return new List<FreeToPlayGamesItens>();

                var url = $"filter?tag={Uri.EscapeDataString(tag)}&platform={Uri.EscapeDataString(platformNormalized)}";
                var list = await _http.GetFromJsonAsync<List<FreeToPlayGamesItens>>(url);

                return list ?? new List<FreeToPlayGamesItens>();
            }) ?? new List<FreeToPlayGamesItens>();
        }
        private static FreeToPlayGamesItens ChooseRecommendedGame(IReadOnlyList<FreeToPlayGamesItens> games)
        {
            int choice = Random.Shared.Next(games.Count);
            return games[choice];
        }

        public static ExternalRecommendedGame MapToExternalRecommendedGame(FreeToPlayGamesItens gameItem)
        {
            return new ExternalRecommendedGame
            {
                Id = gameItem.Id,
                Title = gameItem.Title,
                Genre = gameItem.Genre,
                Platform = gameItem.Platform,
                Url = gameItem.Url
            };
        }

    }
}
