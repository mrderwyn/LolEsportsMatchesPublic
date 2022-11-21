namespace LolEsportsMatches.Services.ErrorStorage
{
    public class ErrorInfo
    {
        public int Id { get; set; }

        public string Category { get; set; } = null!;

        public string InnerId { get; set; } = null!;

        public string Message { get; set; } = null!;
    }
}
