using FlawsFightNightServer.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace FlawsFightNightServer.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = "Host=localhost;Database=flawsfightnight;Username=ffnuser;Password=secret";

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Test database connection BEFORE app.Run()
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                try
                {
                    var tournaments = db.Tournaments.ToList();
                    Console.WriteLine($"Found {tournaments.Count} tournaments in DB");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Database test failed: " + ex.Message);
                }
            }

            // Configure HTTP request pipeline
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
