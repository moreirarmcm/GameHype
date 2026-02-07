using GameHype.Application.DTOs;
using GameHype.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameHype.WebAPI.Controllers
{
    [Route("api/recommendations")]
    [ApiController]
    public class RecommenderController : ControllerBase
    {
        private readonly IGameRecommender _gameRecommender;

        public RecommenderController(IGameRecommender gameRecommender)
        {
            _gameRecommender = gameRecommender;
        }

        [HttpPost]
        public async Task<IActionResult> GetRecommendedGame([FromBody] RecommendedGamesRequest request)
        {
            if (!request.Genres.Any(g => !string.IsNullOrWhiteSpace(g)))
                return BadRequest("Informe, ao menos, um gênero.");

            var platform = request.Platform?.Trim().ToLowerInvariant();
            if (platform != "pc" && platform != "browser")
                platform = "all";
            
            request.Platform = platform;

            if (request.RamMb <= 0)
                request.RamMb = null;

            try
            {
                var recommendedGame = await _gameRecommender.RecommendedGameAsync(request);
                
                if (recommendedGame is null)
                    return NotFound("Nenhum jogo atende aos critérios informados.");

                return Ok(
                    new
                    {
                        Title = recommendedGame?.Title,
                        Url = recommendedGame?.Url
                    });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (HttpRequestException)
            {
                return StatusCode(StatusCodes.Status502BadGateway, "Erro ao consultar serviço externo.");
            }
            catch (TaskCanceledException)
            {
                return StatusCode(StatusCodes.Status504GatewayTimeout, "Tempo limite ao consultar serviço externo.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetRecommendedGamesHistory()
        {
            var history = await _gameRecommender.GetRecommendedGamesHistoryAsync();
            return Ok(history);
        }
    }
}