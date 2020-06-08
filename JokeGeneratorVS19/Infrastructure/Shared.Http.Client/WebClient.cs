using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Shared.Http.Client
{
    public sealed class WebClient : IWebClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WebClient> _logger;
        private readonly UriBuilder _uriBuilder;
        private string _path;

        public WebClient(ILoggerFactory loggerFactory)
        : this(new HttpClient(), loggerFactory)
        {
        }

        public WebClient(HttpClient httpClient, ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = loggerFactory.CreateLogger<WebClient>();
            _httpClient.DefaultRequestHeaders.Add("accept", "application/json");
            _uriBuilder = new UriBuilder();
        }

        public IWebClient WithBaseAddress(string baseUri)
        {
            if (baseUri == null) throw new ArgumentNullException(nameof(baseUri));
            _httpClient.BaseAddress = new Uri(baseUri);
            return this;
        }

        public IWebClient WithParameter(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
                return this;

            var queryToAppend = $"{key}={value}";
            if (_uriBuilder.Query != null && _uriBuilder.Query.Length > 1)
                _uriBuilder.Query = _uriBuilder.Query.Substring(1) + "&" + queryToAppend;
            else
                _uriBuilder.Query = queryToAppend;

            return this;
        }

        public IWebClient WithPath(string path)
        {
            _path = path ?? throw new ArgumentNullException(nameof(path));
            return this;
        }

        public async Task<T[]> GetAsync<T>()
        {
            var query =
                string.IsNullOrWhiteSpace(_path)
                    ? $"{_uriBuilder.Query}"
                    : string.IsNullOrWhiteSpace(_uriBuilder.Query)
                        ? $"{_path}"
                        : $"{_path}/{_uriBuilder.Query}";
            return await GetAsync<T>(query);
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        private async Task<T[]> GetAsync<T>(string query)
        {
            _logger.LogTrace($"Get request with url: {_httpClient.BaseAddress}/{query}");

            var response = await _httpClient.GetStringAsync(query).ConfigureAwait(false);
            return
                response.StartsWith("[")
                    ? JsonConvert.DeserializeObject<T[]>(response)
                    : new T[] { JsonConvert.DeserializeObject<T>(response) };
        }
    }
}