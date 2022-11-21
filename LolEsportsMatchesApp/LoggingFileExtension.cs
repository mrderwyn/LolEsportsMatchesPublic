namespace LolEsportsMatchesApp
{
    public static class LoggingFileExtension
    {
        public static ILoggingBuilder AddFileLogger(this ILoggingBuilder builder, IConfiguration config, string section = "Logging:FileLogging")
        {
            FileLoggingOptions options = new();
            config.Bind(section, options);
            if (options is not null && options.IsValid)
            {
                builder.AddFile(
                    options.PathFormat,
                    options.MinimumLevel,
                    options.LevelOverrides,
                    false,
                    options.FileSizeLimitBytes,
                    options.RetainedFileCountLimit);
            }

            return builder;
        }
    }

    internal class FileLoggingOptions
    {
        public string? PathFormat { get; set; }

        public LogLevel MinimumLevel { get; set; }

        public long FileSizeLimitBytes { get; set; }

        public int RetainedFileCountLimit { get; set; }

        public Dictionary<string, LogLevel> LevelOverrides { get; set; } = new();

        internal bool IsValid =>
            !string.IsNullOrWhiteSpace(PathFormat) && FileSizeLimitBytes != 0 && RetainedFileCountLimit != 0;
    }
}
