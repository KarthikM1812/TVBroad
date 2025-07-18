using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVBroad.Domain.Entities; // Assuming BroadcastSchedule is here

namespace TVBroad.DataAccess.Data
{
        public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Broadcasting> Broadcast { get; set; }

        // Optional: override OnModelCreating for model configuration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // modelBuilder.Entity<BroadcastSchedule>().HasKey(b => b.Id); // Example
        }
    }
}
