using Microsoft.EntityFrameworkCore;
using RpgApp.Shared.Types;

namespace RpgApp.Server.Data
{
    /// <summary>
    /// An entity framework DbContext class is how we interact with a sql database. We have to add
    /// a ConnectionString to our appsettings.json that contains whatever we want the name of our Database
    /// to be. The database will be created and the Tables will be based on the DbSet properties below.
    /// Any object that is a child of one of the DbSets (such as Equipment "Effects" or Player "Attributes") will
    /// also have their own associated database table.
    /// See RpgDbInitializer.cs to see how we use DbContext to interact with the database.
    /// </summary>
    public class RpgAppDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Monster> Monsters { get; set; }
       

        public RpgAppDbContext(DbContextOptions<RpgAppDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>().ToContainer("Players");
            modelBuilder.Entity<Player>().HasKey(p => new {p.UserId, p.Name});
            modelBuilder.Entity<Player>().HasPartitionKey(p => p.UserId);
            modelBuilder.Entity<Player>().HasMany(p => p.Inventory);
            modelBuilder.Entity<Player>().HasMany(p => p.Skills);
            
            modelBuilder.Entity<Equipment>().ToContainer("EquipmentList").HasKey(e => e.Name);
            modelBuilder.Entity<Equipment>().OwnsMany(x => x.Effects);
            modelBuilder.Entity<Skill>().ToContainer("PlayerSkills").HasKey(s => s.Name);
            modelBuilder.Entity<Skill>().OwnsMany(s => s.Effects);
            modelBuilder.Entity<Monster>().ToContainer("Monsters").HasKey(m => m.Name);
            modelBuilder.Entity<Monster>().HasMany(m => m.Treasure);
        }


    }
}
