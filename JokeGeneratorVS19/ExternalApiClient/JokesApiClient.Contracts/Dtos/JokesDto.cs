using Newtonsoft.Json;

namespace JokesApiClient.Contracts.Dtos
{
    public sealed class JokesDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("icon_url")]
        public string IconUrl { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}