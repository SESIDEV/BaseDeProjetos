﻿using BaseDeProjetos;
using BaseDeProjetos.Data;
using System;
using System.Net.Http;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Testes_BaseDeProjetos.Controllers
{
    public class BaseTestes : IClassFixture<BaseApplicationFactory<Startup>>,IDisposable
    {
        protected readonly HttpClient _client;
        protected readonly ApplicationDbContext _context;

        public BaseTestes(BaseApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            var scope = factory.Services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

        public void Dispose()
        {
            // Do "global" teardown here; Called after every test method.
        }

        public static string ToKeyValueURL(object obj)
        {
            var keyvalues = obj.GetType().GetProperties()
                .ToList()
                .Select(p => $"{p.Name} = {p.GetValue(obj)}")
                .ToArray();

            return string.Join('&', keyvalues);
        }
    }
}
