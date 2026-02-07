using GameHype.Application.Interfaces;
using GameHype.Domain.Entities;
using GameHype.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHype.Infrastructure.Repositories
{
    public class GameRecommenderRepository: IGameRecommenderRepository
    {
        private readonly AppDbContext _appContext;

        public GameRecommenderRepository(AppDbContext appContext)
        {
            _appContext = appContext;
        }

        public async Task SaveRecommendedGameAsync(Game game)
        {
           await _appContext.Games.AddAsync(game);

           await _appContext.SaveChangesAsync();

        }

        public async Task<IReadOnlyList<Game>> GetRecommendedGamesHistoryAsync()
        {
            return await _appContext.Games.AsNoTracking().OrderByDescending(x => x.RecommendedAt).ToListAsync();
        }
    }
}
