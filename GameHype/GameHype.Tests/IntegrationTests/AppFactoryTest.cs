using GameHype.Application.Clients.FreeToPlay.Interfaces;
using GameHype.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHype.Tests.IntegrationTests
{
    public sealed class AppFactoryTest : WebApplicationFactory<Program>
    {
        private DbConnection? _connection;
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                if (dbContextDescriptor is not null)
                    services.Remove(dbContextDescriptor);
                
                _connection = new SqliteConnection("Data Source=:memory:");
                _connection.Open();

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseSqlite(_connection);
                });

                var clientDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IFreeToPlayClient));
                if (clientDescriptor is not null)
                    services.Remove(clientDescriptor);

                services.AddSingleton<IFreeToPlayClient, FakeFreeToPlayClient>();

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _connection?.Dispose();
                _connection = null;
            }
        }
    }
}
