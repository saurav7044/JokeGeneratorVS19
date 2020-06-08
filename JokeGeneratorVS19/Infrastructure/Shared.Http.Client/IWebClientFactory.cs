namespace Shared.Http.Client
{
    public interface IWebClientFactory
    {
        IWebClient Create();
    }
}