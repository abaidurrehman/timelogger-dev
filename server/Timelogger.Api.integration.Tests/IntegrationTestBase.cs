using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Timelogger.Api.integration.Tests
{
    public class IntegrationTestBase
    {
        protected readonly HttpClient Client;
        protected readonly TestServer Server;

        public IntegrationTestBase()
        {
            Server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            Client = Server.CreateClient();
        }
    }
}