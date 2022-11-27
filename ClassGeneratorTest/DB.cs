using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassGeneratorTest
{
    public class DB : DbContext
    {
        Type _dynamicType;
        public DB(Type dynamicType)
        {
            _dynamicType = dynamicType;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("TrustServerCertificate=True;Persist Security Info = False; User ID =test; Password =test; Initial Catalog =agile_config; Server =.");
            optionsBuilder.UseNpgsql("Host=192.168.0.125;port=15432;Database=testdb;Username=postgres;Password=123456");
            base.OnConfiguring(optionsBuilder);
         }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity(_dynamicType).ToTable("app").HasKey("Id");
            base.OnModelCreating(modelBuilder);
        }

        //public DbSet<App> Apps {get;set;}
    }


    [Table("app")]
    public class App
    {
        [Column("id")]
        public string Id { get; set; }
    }
}
