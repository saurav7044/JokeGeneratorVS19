using System.Collections.Generic;
using System.Threading.Tasks;
using JokesApiClient.Contracts.Dtos;

namespace JokesApiClient.Contracts
{
    public interface IJokeWebApiClient
    {
        Task<IList<JokesDto>> GetJokesAsync(int jokesAmount, string category = null, string name = null);

        Task<PersonDto[]> GetPersonInfoAsync(int count);

        Task<string[]> GetCategoriesAsync();
    }
}