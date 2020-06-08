using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Shared.Http.Client;

namespace JokesApiClient.Tests
{
    [TestClass]
    public class JokeWebApiClientGetJokesTest
    {
        private static IOptions<JokesSettings> GetOptions()
        {
            var options = Substitute.For<IOptions<JokesSettings>>();
            options.Value.Returns(new JokesSettings
            {
                NamesUrl = "https://api.chucknorris.io",
                ChucknorrisUrl = "https://api.chucknorris.io"
            });
            return options;
        }

        [DataTestMethod]
        [DataRow(1)]        
        [DataRow(5)]
        public async Task GetRenamedFirstNameOnlyJokesTest(int count)
        {
            var options = GetOptions();
            var jokeProvider = new JokeWebApiClient(GetWebClientFactory(), options);
            var results = await jokeProvider.GetJokesAsync(count, "history", "Thomas");
            Assert.AreEqual(count, results.Count);
        }

        [DataTestMethod]
        [DataRow(1)]        
        [DataRow(5)]
        public async Task GetRenamedLastNameOnlyJokesTest(int count)
        {
            var options = GetOptions();
            var jokeProvider = new JokeWebApiClient(GetWebClientFactory(), options);
            var results = await jokeProvider.GetJokesAsync(count, "food", "Crane");
            Assert.AreEqual(count, results.Count);
        }

        [DataTestMethod]
        [DataRow(1)]        
        [DataRow(5)]
        public async Task GetRenamedBothNamesJokesTest(int count)
        {
            var options = GetOptions();
            var jokeProvider = new JokeWebApiClient(GetWebClientFactory(), options);
            var results = await jokeProvider.GetJokesAsync(count, "food", "Thomas Crane");
            Assert.AreEqual(count, results.Count);
        }

        [DataTestMethod]
        [DataRow(1)]        
        [DataRow(5)]
        public async Task GetNoNameJustCategoryJokesTest(int count)
        {
            var options = GetOptions();
            var jokeProvider = new JokeWebApiClient(GetWebClientFactory(), options);
            var results = await jokeProvider.GetJokesAsync(count, "movie");
            Assert.AreEqual(count, results.Count);
        }

        [DataTestMethod]
        [DataRow(1)]        
        [DataRow(5)]
        public async Task GetAnyJokesTest(int count)
        {
            var options = GetOptions();
            var jokeProvider = new JokeWebApiClient(GetWebClientFactory(), options);
            var results = await jokeProvider.GetJokesAsync(count);
            Assert.AreEqual(count, results.Count);
        }

        private static WebClientFactory GetWebClientFactory()
        {
            var logFactory = Substitute.For<ILoggerFactory>();
            return new WebClientFactory(logFactory);
        }
    }
}