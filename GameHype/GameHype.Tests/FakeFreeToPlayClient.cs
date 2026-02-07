using GameHype.Application.Clients.FreeToPlay;
using GameHype.Application.Clients.FreeToPlay.Interfaces;
using GameHype.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHype.Tests
{
    public class FakeFreeToPlayClient: IFreeToPlayClient
    {
        public Task<ExternalRecommendedGame?> GetRecommendedGameAsync(List<string> genre, string? platform, int? ramMb)
        {
            return Task.FromResult<ExternalRecommendedGame?>(new ExternalRecommendedGame
            {
                Id = 296,
                Title = "S4 league",
                Url = "https://www.freetogame.com/open/s4-league",
                Genre = "Shooter",
                Platform = "pc"
            });
        }

        public Task<RecommendedGamesDetailsResponse> GetGameById(int id)
        {
            return Task.FromResult(new RecommendedGamesDetailsResponse
            {
                Id = id,
                MinimumRamMb = 4096,
            });
        }
    }
}
