using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace LolEsportsMatches.Services.ErrorStorage.Logger
{
    /// <summary>
    /// Static class for AddErrorJsonLogger extension.
    /// </summary>
    public static class ErrorJsonLoggerExtension
    {
        /// <summary>
        /// Adds the error json logger.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static ILoggingBuilder AddErrorJsonLogger(
            this ILoggingBuilder builder)
        {
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<ILoggerProvider, ErrorJsonLoggerProvider>());

            LoggerProviderOptions.RegisterProviderOptions
                <ErrorJsonLoggerConfiguration, ErrorJsonLoggerProvider>(builder.Services);
            return builder;
        }

        /// <summary>
        /// Adds the error json logger.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="configure">The delegate to get configure.</param>
        /// <returns></returns>
        public static ILoggingBuilder AddErrorJsonLogger(
            this ILoggingBuilder builder,
            Action<ErrorJsonLoggerConfiguration> configure)
        {
            builder.AddErrorJsonLogger();
            builder.Services.Configure(configure);

            return builder;
        }
    }
}