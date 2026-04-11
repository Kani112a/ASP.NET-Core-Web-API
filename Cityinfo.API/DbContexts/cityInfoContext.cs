using Cityinfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cityinfo.API.DbContexts
{
    public class cityInfoContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<pointOfInterest> pointOfInterests { get; set; }
        public cityInfoContext(DbContextOptions<cityInfoContext> options) : base(options)
        {

        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("connectionstring");
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
