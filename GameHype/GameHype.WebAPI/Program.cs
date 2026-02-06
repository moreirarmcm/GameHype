
using GameHype.Application;
using GameHype.Application.Clients.FreeToPlay.Interfaces;
using GameHype.Application.Interfaces;
using GameHype.Infrastructure.Clients.FreeToPlay;
using GameHype.Infrastructure.Clients.FreeToPlay.Caching;
using GameHype.Infrastructure.Persistence;
using GameHype.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GameHype.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddMemoryCache();
            builder.Services.Configure<FreeToPlayCacheParams>(static opt =>
            {

            });
            builder.Services.Configure<FreeToPlayCacheParams>(par =>
                new FreeToPlayCacheParams
                {
                    FilterTtl = TimeSpan.FromMinutes(10),
                    GameDetailsTtl = TimeSpan.FromMinutes(60)
                });

            builder.Services.AddHttpClient<IFreeToPlayClient, FreeToPlayClient>(http =>
            {
                http.BaseAddress = new Uri("https://www.freetogame.com/api/");
                http.Timeout = TimeSpan.FromSeconds(10);
            });

            builder.Services.AddScoped<IGameRecommender, GameRecommender>();
            builder.Services.AddScoped<IGameRecommenderRepository, GameRecommenderRepository>();
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=GameHype.db"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
