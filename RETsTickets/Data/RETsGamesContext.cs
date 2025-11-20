using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RETsGames.Models;

namespace RETsGames.Data
{
    public class RETsGamesContext : DbContext
    {
        public RETsGamesContext (DbContextOptions<RETsGamesContext> options)
            : base(options)
        {
        }

        public DbSet<RETsGames.Models.Game> Game { get; set; } = default!;
        public DbSet<RETsGames.Models.Category> Category { get; set; } = default!;

        // Renamed DbSet
        public DbSet<RETsGames.Models.Purchase> Purchases { get; set; } = default!;
    }
}
