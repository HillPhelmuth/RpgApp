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
    public class RpgDbContext : DbContext
    {
        /// <summary>
        /// DbSet<typeparam name="Player"></typeparam> is how we will create and query the Players table in our sql DB
        /// </summary>
        public DbSet<Player> Players { get; set; }
        /// <summary>
        /// DbSet<typeparam name="Equipment"></typeparam> is how we will create and query the Equipment table in our sql DB
        /// </summary>
        public DbSet<Equipment> Equipment { get; set; }
        /// <summary>
        /// DbSet<typeparam name="Skills"></typeparam> is how we will create and query the Skills table in our sql DB
        /// </summary>
        public DbSet<Skill> Skills { get; set; }
        //public DbSet<PlayerSkill> PlayerSkills { get; set; }
        //public DbSet<PlayerEquipment> PlayerInventory { get; set; }
        public DbSet<Monster> Monsters { get; set; }

        public RpgDbContext(DbContextOptions<RpgDbContext> options) 
            : base(options)
        {

        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<PlayerSkill>()
        //        .HasKey(cs => new { cs.PlayerID, cs.SkillID });
        //    modelBuilder.Entity<PlayerEquipment>()
        //        .HasKey(cs => new {cs.PlayerID, cs.EquipmentId});
        //}
    }
}
