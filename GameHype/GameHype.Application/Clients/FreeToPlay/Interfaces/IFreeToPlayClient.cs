using GameHype.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHype.Application.Clients.FreeToPlay.Interfaces
{
    public interface IFreeToPlayClient
    {
        Task <ExternalRecommendedGame?> GetRecommendedGameAsync(List<string> genre, string? platform, int? ramMb);

        Task <RecommendedGamesDetailsResponse> GetGameById(int id);
    }
}
