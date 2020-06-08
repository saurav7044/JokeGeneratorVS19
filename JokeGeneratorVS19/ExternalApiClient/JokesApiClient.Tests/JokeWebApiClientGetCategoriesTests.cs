using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Shared.Http.Client;

namespace JokesApiClient.Tests
{
    [TestClass]
    public class JokeWebApiClientGetCategoriesTests
    {
        [TestMethod]
        public async Task GetAllCategoriesTest()
        {
            var logFactory = Substitute.For<ILoggerFactory>();
            var options = Substitute.For<IOptions<JokesSettings>>();
            options.Value.Returns(new JokesSettings
            {
                NamesUrl = "https://api.chucknorris.io",
                ChucknorrisUrl = "https://api.chucknorris.io"
            });
            var categoryProvider = new JokeWebApiClient(new WebClientFactory(logFactory), options);
            var result = await categoryProvider.GetCategoriesAsync();

            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Length);
        }
    }
}