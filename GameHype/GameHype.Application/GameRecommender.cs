using GameHype.Application.Clients.FreeToPlay;
using GameHype.Application.Clients.FreeToPlay.Interfaces;
using GameHype.Application.DTOs;
using GameHype.Application.Interfaces;
using GameHype.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GameHype.Application
{
    public class GameRecommender : IGameRecommender
    {
        private readonly IFreeToPlayClient _freeToPlayClient;
        private readonly IGameRecommenderRepository _gameRecommenderRepository;

        public GameRecommender(IFreeToPlayClient freeToPlayClient, IGameRecommenderRepository gameRecommenderRepository)
        {
            _freeToPlayClient = freeToPlayClient;
            _gameRecommenderRepository = gameRecommenderRepository;
        }
        public async Task<RecommendedGamesResponse?> RecommendedGameAsync(RecommendedGamesRequest request)
        {
            if (request.Genres == null || request.Genres.Count == 0)
                throw new ArgumentException("É necessário especificar, ao menos, um gênero.", nameof(request.Genres));

            var chosenGame = await _freeToPlayClient.GetRecommendedGameAsync(request.Genres, request.Platform, request.RamMb);

            if (chosenGame == null)
                return null;
            
            var game = new Game
            {
                FreeToPlayId = chosenGame.Id,
                Title = chosenGame.Title,
                Genre = chosenGame.Genre,
                RecommendedAt = DateTime.UtcNow
            };

            await SaveRecommendedGameAsync(game);
            var recommendedGamesResponse = new RecommendedGamesResponse
            {
                Title = chosenGame.Title,
                Url = chosenGame.Url,
            };

            return recommendedGamesResponse;
        }

        private async Task SaveRecommendedGameAsync(Game game)
        {
           await _gameRecommenderRepository.SaveRecommendedGameAsync(game);
        }

        public async Task<IEnumerable<RecommendedGamesHistoryResponse?>> GetRecommendedGamesHistoryAsync()
        {
            var games = await _gameRecommenderRepository.GetRecommendedGamesHistoryAsync();
           
            if (games == null || !games.Any())
                return Enumerable.Empty<RecommendedGamesHistoryResponse>();

            return games.Select(g => new RecommendedGamesHistoryResponse
            {
                FreeToPlayId = g.FreeToPlayId,
                Title = g.Title,
                Genre = g.Genre,
                RecommendedAt = g.RecommendedAt
            });
        }
    }
}
