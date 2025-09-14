using FlawsFightNightServer.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlawsFightNightServer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Guild> Guilds { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Member> Members { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Guild -> Tournament (1-to-many)
            modelBuilder.Entity<Guild>()
                .HasMany(t => t.Tournaments)
                .WithOne(g => g.Guild)
                .HasForeignKey(k => k.GuildId)
                .OnDelete(DeleteBehavior.Cascade);

            // Tournament -> Team (1-to-many)
            modelBuilder.Entity<Tournament>()
                .HasMany(t => t.Teams)
                .WithOne(team => team.Tournament)
                .HasForeignKey(k => k.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Team -> Member (1-to-many)
            modelBuilder.Entity<Team>()
                .HasMany(t => t.Members)
                .WithOne(m => m.Team)
                .HasForeignKey(k => k.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            // TODO Tournament -> MatchLog (1-to-many)


            // TODO MatchLog -> Entry (1-to-many)

            // Primary Keys
            modelBuilder.Entity<Guild>().HasKey(g => g.Id);
            modelBuilder.Entity<Tournament>().HasKey(t => t.Id);
            modelBuilder.Entity<Team>().HasKey(t => t.Id);
            modelBuilder.Entity<Member>().HasKey(m => m.Id);
            // modelBuilder.Entity<MatchLog>().HasKey(m => m.Id);
            // modelBuilder.Entity<Entry>().HasKey(e => e.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
