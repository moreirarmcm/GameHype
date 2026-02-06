using GameHype.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHype.Application.Interfaces
{
    public interface IGameRecommender
    {
        Task <RecommendedGamesResponse?> RecommendedGameAsync(RecommendedGamesRequest request);

        Task <IEnumerable<RecommendedGamesHistoryResponse?>> GetRecommendedGamesHistoryAsync();
    }
}
