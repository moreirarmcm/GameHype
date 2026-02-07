using GameHype.Application;
using GameHype.Application.Clients.FreeToPlay;
using GameHype.Application.Clients.FreeToPlay.Interfaces;
using GameHype.Application.DTOs;
using GameHype.Application.Interfaces;
using GameHype.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GameHype.Tests.UnitTests
{
    public class GameRecommenderTest
    {
        private readonly Mock<IFreeToPlayClient> _client = new();
        private readonly Mock<IGameRecommenderRepository> _repository = new();

        private IGameRecommender CreateSut() => new GameRecommender(_client.Object, _repository.Object );

        [Fact]
        public async Task RecommendedGameAsync_ClientReturnNull_ShouldReturnsNullAndNotSave()
        {
            var sut = CreateSut();
            var request = new RecommendedGamesRequest
            {
                Genres = ["mmo", "shooter"],
                Platform = "",
                RamMb = 4000
            };
            _client.Setup(c => c.GetRecommendedGameAsync(request.Genres, request.Platform, request.RamMb))
           .ReturnsAsync((ExternalRecommendedGame?)null);

            var result = await sut.RecommendedGameAsync(request);
            Assert.Null(result);
            _repository.Verify(r => r.SaveRecommendedGameAsync(It.IsAny<Game>()), Times.Never);
        }
        
        [Fact]
        public async Task RecommendGameAsync_ValidReturn_ShouldReturnTitleAndUrl_AndSaveHistory()
        {
            var sut = CreateSut();

            var request = new RecommendedGamesRequest
            {
                Genres = { "shooter" },
                Platform = "all",
                RamMb = null
            };

            var external = new ExternalRecommendedGame
            {
                Id = 296,
                Title = "S4 league",
                Url = "https://www.freetogame.com/open/s4-league",
                Genre = "Shooter",
                Platform = "pc"
            };

            _client.Setup(c => c.GetRecommendedGameAsync(request.Genres, request.Platform, request.RamMb))
                   .ReturnsAsync(external);

            var result = await sut.RecommendedGameAsync(request);

            Assert.Equal("S4 league", result.Title);
            Assert.Equal("https://www.freetogame.com/open/s4-league", result.Url);

            _repository.Verify(r => r.SaveRecommendedGameAsync(It.Is<Game>(g =>
                g.FreeToPlayId == 296 &&
                g.Title == "S4 league" &&
                g.Genre == "Shooter" &&
                g.RecommendedAt != default
            )), Times.Once);
        }

        [Fact]
        public async Task RecommendedGamesHistory_ReturnWithData()
        {
            var sut = CreateSut();

            var stored = new List<Game>
            {
                new Game
                {
                    Id = 1,
                    FreeToPlayId = 296,
                    Title = "S4 league",
                    Genre = "Shooter",
                    RecommendedAt = DateTime.UtcNow
                }
            };

            _repository.Setup(r => r.GetRecommendedGamesHistoryAsync())
                 .ReturnsAsync(stored);

            var result = await sut.GetRecommendedGamesHistoryAsync();

            var item = Assert.Single(result);
            Assert.Equal(296, item.FreeToPlayId);
            Assert.Equal("S4 league", item.Title);
            Assert.Equal("Shooter", item.Genre);
        }

        [Fact]
        public async Task RecommendedGamesHistory_ReturnEmpty()
        {
            var sut = CreateSut();

            _repository.Setup(r => r.GetRecommendedGamesHistoryAsync())
                 .ReturnsAsync(new List<Game>());
            
            Assert.Empty(await sut.GetRecommendedGamesHistoryAsync());
        }
    }
}
