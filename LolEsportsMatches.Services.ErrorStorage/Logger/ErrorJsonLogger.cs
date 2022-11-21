using Microsoft.Extensions.Logging;

namespace LolEsportsMatches.Services.ErrorStorage.Logger
{
    /// <summary>
    /// Logger for important errors to track.
    /// </summary>
    /// <remarks>
    /// Log only errors and criticals, only from the services specified in configuration.
    /// Use <see cref="ErrorStorage"/> as output to write logs.
    /// </remarks>
    /// <seealso cref="Microsoft.Extensions.Logging.ILogger" />
    public sealed class ErrorJsonLogger : ILogger
    {
        private readonly string name;
        private readonly Func<ErrorJsonLoggerConfiguration> getCurrentConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorJsonLogger"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="getCurrentConfig">The delegate to get configuration.</param>
        public ErrorJsonLogger(string name, Func<ErrorJsonLoggerConfiguration> getCurrentConfig)
        {
            this.name = name;
            this.getCurrentConfig = getCurrentConfig;
        }

        /// <summary>
        /// Begins a logical operation scope.
        /// </summary>
        /// <typeparam name="TState">The type of the state to begin scope for.</typeparam>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>
        /// An <see cref="T:System.IDisposable" /> that ends the logical operation scope on dispose.
        /// </returns>
        public IDisposable BeginScope<TState>(TState state) => default!;

        /// <summary>
        /// Checks if the given <paramref name="logLevel" /> is enabled.
        /// </summary>
        /// <param name="logLevel">Level to be checked.</param>
        /// <returns>
        ///   true if enabled, otherwise - false.
        /// </returns>
        public bool IsEnabled(LogLevel logLevel) => (logLevel == LogLevel.Error || logLevel == LogLevel.Critical) &&
            this.getCurrentConfig().ServiceToCategoryMap.ContainsKey(this.name);

        /// <summary>
        /// Writes a log entry (only errors and criticals, from specified services).
        /// </summary>
        /// <typeparam name="TState">The type of the object to be written.</typeparam>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="state">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a <see cref="T:System.String" /> message of the <paramref name="state" /> and <paramref name="exception" />.</param>
        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!this.IsEnabled(logLevel))
            {
                return;
            }

            ErrorInfo? error = new()
            {
                Category = this.getCurrentConfig().ServiceToCategoryMap[this.name],
                InnerId = "none",
                Message = formatter(state, exception)
            };

            int from = error.Message.IndexOf('#');
            if (from != -1)
            {
                int to = error.Message.IndexOfAny(new[] { ' ', ',', '!', '.', '(', ')', ':', ';' }, from);
                if (to != -1)
                {
                    error.InnerId = error.Message.Substring(from + 1, to - from);
                }
            }

            ErrorStorage? storage = new(this.getCurrentConfig().FilePath);
            storage.AddError(error);
        }
    }
}
