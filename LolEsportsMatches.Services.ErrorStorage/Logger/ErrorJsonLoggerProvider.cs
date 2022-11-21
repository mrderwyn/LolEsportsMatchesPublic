using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace LolEsportsMatches.Services.ErrorStorage.Logger
{
    /// <summary>
    /// <see cref="ErrorJsonLogger"/> provider.
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Logging.ILoggerProvider" />
    [ProviderAlias("ErrorJson")]
    public sealed class ErrorJsonLoggerProvider : ILoggerProvider
    {
        private readonly IDisposable onChangeToken;
        private ErrorJsonLoggerConfiguration currentConfig;
        private readonly ConcurrentDictionary<string, ErrorJsonLogger> loggers =
            new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorJsonLoggerProvider"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public ErrorJsonLoggerProvider(IOptionsMonitor<ErrorJsonLoggerConfiguration> config)
        {
            currentConfig = config.CurrentValue;
            onChangeToken = config.OnChange(updatedConfig => currentConfig = updatedConfig);
        }

        /// <summary>
        /// Creates a new <see cref="T:Microsoft.Extensions.Logging.ILogger" /> instance for specified category.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        /// <returns>
        /// The instance of <see cref="T:Microsoft.Extensions.Logging.ILogger" /> that was created.
        /// </returns>
        public ILogger CreateLogger(string categoryName)
        {
            return this.loggers.GetOrAdd(categoryName, name => new ErrorJsonLogger(name, GetCurrentConfig));
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.loggers.Clear();
            onChangeToken.Dispose();
        }

        private ErrorJsonLoggerConfiguration GetCurrentConfig() => currentConfig;
    }
}
