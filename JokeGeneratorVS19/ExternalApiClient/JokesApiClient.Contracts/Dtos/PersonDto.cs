using Newtonsoft.Json;

namespace JokesApiClient.Contracts.Dtos
{
    public class PersonDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }
    }
}