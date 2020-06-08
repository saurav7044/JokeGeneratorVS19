using System;
using System.Threading.Tasks;

namespace Shared.Http.Client
{
    public interface IWebClient : IDisposable
    {
        IWebClient WithBaseAddress(string baseUri);
        IWebClient WithParameter(string key, string value);
        IWebClient WithPath(string path);
        Task<T[]> GetAsync<T>();
    }
}