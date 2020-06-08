using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Shared.Http.Client;

namespace JokesApiClient.Tests
{
    [TestClass]
    public class JokeWebApiClientGetNamesTests
    {
        [DataTestMethod]
        [DataRow(1)]
        public async Task GetNamesTest(int count)
        {
            var options = GetOptions();
            var provider = new JokeWebApiClient(new WebClientFactory(Substitute.For<ILoggerFactory>()), options);
            var result = await provider.GetPersonInfoAsync(count);

            Assert.AreEqual(count, result.Length);
        }

        private static IOptions<JokesSettings> GetOptions()
        {
            var options = Substitute.For<IOptions<JokesSettings>>();
            options.Value.Returns(new JokesSettings
            {
                NamesUrl = "https://names.privserv.com/api/",
                ChucknorrisUrl = "https://api.chucknorris.io/"
            });
            return options;
        }
    }
}