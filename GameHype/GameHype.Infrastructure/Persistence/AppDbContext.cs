using GameHype.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHype.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Game> Games => Set<Game>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>(e =>
            {
                e.ToTable("Games");
                e.HasKey(x => x.Id);
                e.Property(x => x.FreeToPlayId).IsRequired();
                e.Property(x => x.Title).IsRequired().HasMaxLength(200);
                e.Property(x => x.Genre).IsRequired().HasMaxLength(80);
                e.Property(x => x.RecommendedAt).IsRequired();
            });
        }
    }
}

    