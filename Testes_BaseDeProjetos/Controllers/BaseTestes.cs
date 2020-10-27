using BaseDeProjetos;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;

namespace Testes_BaseDeProjetos.Controllers
{
    public class BaseTestes : IClassFixture<WebApplicationFactory<Startup>>
    {
        protected readonly HttpClient _client;

        public BaseTestes(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }
    }
}
