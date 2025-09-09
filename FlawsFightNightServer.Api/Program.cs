using FlawsFightNightServer.Core.Managers;
using FlawsFightNightServer.Data;
using FlawsFightNightServer.Data.Handlers;
using Microsoft.EntityFrameworkCore;
using System;

namespace FlawsFightNightServer.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            /////////// Register my services ///////////
            // Managers
            builder.Services.AddSingleton<DataManager>();
            builder.Services.AddSingleton<TournamentManager>();

            // Data Handlers
            builder.Services.AddSingleton<TournamentsDatabaseHandler>();

            // Build the app
            var app = builder.Build();

            // Configure HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            // Initialize DataManager to load data at startup
            DataManager dataManager = app.Services.GetRequiredService<DataManager>();

            app.Run();
        }
    }
}
