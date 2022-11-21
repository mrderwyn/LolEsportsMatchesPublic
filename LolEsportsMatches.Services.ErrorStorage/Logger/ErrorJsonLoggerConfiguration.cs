namespace LolEsportsMatches.Services.ErrorStorage.Logger
{
    public class ErrorJsonLoggerConfiguration
    {
        public string FilePath { get; set; } = "important-errors.json";

        public Dictionary<string, string> ServiceToCategoryMap { get; set; } = new();
    }
}
