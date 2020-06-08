using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;


namespace Shared.Http.Client.Tests
{
    [TestClass]
    public sealed class WebClientTests
    {
        public static string Url = "http://localhost:15001";
        public const string RoutePath = "api/v1/test1";
        public const string Result = "test result";

        [TestMethod]
        public async Task GetAllCategoriesTest()
        {
            using var server = BuildTestServer();
            var httpClient = server.CreateClient();
            using var webClient = new WebClient(httpClient, Substitute.For<ILoggerFactory>())
                .WithBaseAddress(Url)
                .WithPath(RoutePath)
                .WithParameter("key", "test")
                .WithParameter("name", "test");
            var result = await webClient.GetAsync<string>();

            Assert.AreEqual(result.Single(), Result);
        }

        private static TestServer BuildTestServer()
        {
            return new TestServer(
                new WebHostBuilder()
                    .ConfigureServices(
                        collection =>
                        {
                            collection.AddMvcCore()
                                .ConfigureApplicationPartManager(
                                    manager => manager.ApplicationParts.Add(
                                        new AssemblyPart(typeof(TestController).Assembly)));
                            collection.AddControllers(options => options.EnableEndpointRouting = false);
                            collection.AddTransient<WebClient>();
                        })
                    .Configure(builder => builder.UseMvc()))
            {
                BaseAddress = new Uri(Url)
            };
        }
    }

    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Route(WebClientTests.RoutePath)]
        public IActionResult Get(string key, string name)
        {
            return Ok(WebClientTests.Result);
        }
    }
}