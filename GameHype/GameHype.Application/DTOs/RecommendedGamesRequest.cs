namespace GameHype.Application.DTOs
{
    public class RecommendedGamesRequest
    {
        public List<string> Genres { get; set; } = new();
        public string? Platform { get; set; } = string.Empty;
        public int? RamMb { get; set; } = null;
    }
}
