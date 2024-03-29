﻿using BaseDeProjetos;
using BaseDeProjetos.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace Testes_BaseDeProjetos.Controllers
{
    public class BaseTestes : IClassFixture<BaseApplicationFactory<Startup>>, IDisposable
    {
        protected readonly HttpClient _client;
        protected readonly ApplicationDbContext _context;

        public BaseTestes(BaseApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            IServiceScope scope = factory.Services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

        public void Dispose()
        {
            // Do "global" teardown here; Called after every test method.
        }

        public static FormUrlEncodedContent ToKeyValueURL(object obj)
        {
            KeyValuePair<string, string>[] keyvalues = obj.GetType().GetProperties()
                .ToList()
                    .Select(p => new KeyValuePair<string, string>(p.Name, p.GetValue(obj)?.ToString()))
                            .ToArray();

            FormUrlEncodedContent content = new FormUrlEncodedContent(keyvalues);

            return content;
        }
    }
}