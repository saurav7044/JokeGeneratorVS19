using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JokesApiClient.Contracts;
using JokesApiClient.Contracts.Dtos;
using Microsoft.Extensions.Options;
using Shared.Http.Client;

namespace JokesApiClient
{
    /// <summary>
    /// Represent remote facade to communicate with remote server.
    /// </summary>
    public sealed class JokeWebApiClient : IJokeWebApiClient
    {
        private readonly IWebClientFactory _webClientFactory;
        private readonly IOptions<JokesSettings> _options;

        public JokeWebApiClient(IWebClientFactory webClientFactory, IOptions<JokesSettings> options)
        {
            _webClientFactory = webClientFactory;
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<IList<JokesDto>> GetJokesAsync(int jokesAmount, string category = null, string name = null)
        {
            var result = new List<JokesDto>();
            using var client = _webClientFactory.Create()
                .WithBaseAddress(_options.Value.ChucknorrisUrl)
                .WithPath("jokes/random")
                .WithParameter("name", name)
                .WithParameter("category", category);
            for (var i = 0; i < jokesAmount; i++)
            {
                var jokes = await client.GetAsync<JokesDto>();
                result.AddRange(jokes);
            }

            return result;
        }

        public async Task<PersonDto[]> GetPersonInfoAsync(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Cannot generate a negative number of names");
            
            using var client = _webClientFactory.Create()
                .WithBaseAddress(_options.Value.NamesUrl)
                .WithParameter("amount", count.ToString());
            return await client.GetAsync<PersonDto>();
        }

        public async Task<string[]> GetCategoriesAsync()
        {
            using var client = _webClientFactory.Create()
                .WithBaseAddress(_options.Value.ChucknorrisUrl)
                .WithPath("jokes/categories");
            return await client.GetAsync<string>();
        }
    }
}