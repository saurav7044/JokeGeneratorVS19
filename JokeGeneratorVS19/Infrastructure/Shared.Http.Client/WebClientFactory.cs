using System;
using Microsoft.Extensions.Logging;

namespace Shared.Http.Client
{
    public sealed class WebClientFactory : IWebClientFactory
    {
        private readonly ILoggerFactory _loggerFactory;

        public WebClientFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public IWebClient Create()
        {
            return new WebClient(_loggerFactory);
        }
    }
}