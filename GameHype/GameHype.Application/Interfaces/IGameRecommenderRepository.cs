using GameHype.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHype.Application.Interfaces
{
    public interface IGameRecommenderRepository
    {
        Task SaveRecommendedGameAsync(Game game);
        Task <IReadOnlyList<Game>> GetRecommendedGamesHistoryAsync();
    }
}
