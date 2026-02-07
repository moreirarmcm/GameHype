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
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GameHype.Tests.IntegrationTests
{
    public sealed class RecommendationsFlowTests : IClassFixture<AppFactoryTest>
    {
        private readonly HttpClient _client;

        public RecommendationsFlowTests(AppFactoryTest factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_GetPersist_ReturnsHistory()
        {
            var body = new
            {
                genres = new[] { "shooter" },
                platform = "all",
                ramMb = (int?)null
            };

            var post = await _client.PostAsJsonAsync("/api/recommendations", body);
            Assert.Equal(HttpStatusCode.OK, post.StatusCode);

            var postJson = await post.Content.ReadFromJsonAsync<RecommendedGameTestResponse>();
            Assert.NotNull(postJson);
            Assert.Equal("S4 league", postJson!.Title);
            Assert.Equal("https://www.freetogame.com/open/s4-league", postJson.Url);

            var get = await _client.GetAsync("/api/recommendations");
            Assert.Equal(HttpStatusCode.OK, get.StatusCode);

            var history = await get.Content.ReadFromJsonAsync<List<RecommendedGamesHistoryTestResponse>>();
            Assert.NotNull(history);
            Assert.True(history!.Count >= 1);

            Assert.Contains(history, h => h.Title == "S4 league");
        }

        private sealed class RecommendedGameTestResponse
        {
            public string Title { get; set; } = "";
            public string Url { get; set; } = "";
        }

        private sealed class RecommendedGamesHistoryTestResponse
        {
            public string Title { get; set; } = "";
            public string Genre { get; set; } = "";
            public DateTime RecommendedAt { get; set; }
        }
    }
}