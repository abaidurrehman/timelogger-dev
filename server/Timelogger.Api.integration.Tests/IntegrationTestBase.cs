using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting;

namespace Timelogger.Api.integration.Tests
{
    public class IntegrationTestBase
    {
        protected readonly TestServer Server;
        protected readonly HttpClient Client;

        public IntegrationTestBase()
        {
            Server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            Client = Server.CreateClient();
        }
    }
}
