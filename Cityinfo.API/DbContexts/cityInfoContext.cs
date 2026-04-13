using Cityinfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cityinfo.API.DbContexts
{
    public class cityInfoContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<pointOfInterest> PointsOfInterest { get; set; }
        public cityInfoContext(DbContextOptions<cityInfoContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(
                new City("Dharmapuri")
                {
                    Id = 1,
                    Description = "Hogenakkal Falls"
                },
                new City("Salem")
                {
                    Id = 2,
                    Description = "House of the seven Gables"
                },
                new City("Erode")
                {
                    Id = 3,
                    Description = "Handloom textiles"
                }
            );
            modelBuilder.Entity<pointOfInterest>().HasData(
                new pointOfInterest("Theerthamalai Temple")
                {
                    Id = 1,
                    CityId = 1,
                    Description = "It blends hill heritage with a calm elevation walk"
                },
                new pointOfInterest("Adhiyamankottai Fort")
                {
                    Id = 2,
                    CityId = 1,
                    Description = "Carrying the district’s medieval stone-defense identity"
                },
                new pointOfInterest("Yercaud")
                {
                    Id = 3,
                    CityId = 2,
                    Description = "Tourist Place"
                },
                new pointOfInterest("Mettur Dam")
                {
                    Id = 4,
                    CityId = 2,
                    Description = "One of the most famous place"
                },
                new pointOfInterest("Thindal Murugan Temple")
                {
                    Id = 5,
                    CityId = 3,
                    Description = "Devotional Place"
                },
                new pointOfInterest("Vellode Bird Sanctuary")
                {
                    Id = 6,
                    CityId = 3,
                    Description = "Sanctuary with boat riding"
                }
                );
            base.OnModelCreating(modelBuilder);
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("connectionstring");
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
