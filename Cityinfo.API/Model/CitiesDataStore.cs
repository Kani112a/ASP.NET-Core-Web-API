namespace Cityinfo.API.Model
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }
        public static CitiesDataStore Current { get; } = new CitiesDataStore();
        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "Dharmapuri",
                    Description = "Hogenakkal Falls",
                    PointsOfInterest = new List<PointOfInterestDto>()   // child resource
                    {
                        new PointOfInterestDto()
                        {
                            Id=1,
                            Name="",
                            Description=""
                        },
                        new PointOfInterestDto()
                        {
                            Id=2,
                            Name="",
                            Description=""
                        }
                    }
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Salem",
                    Description = "House of the seven Gables",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id=3,
                            Name="",
                            Description=""
                        },
                        new PointOfInterestDto()
                        {
                            Id=4,
                            Name="",
                            Description=""
                        }
                    }
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Erode",
                    Description = "Handloom textiles",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id=5,
                            Name="",
                            Description=""
                        },
                        new PointOfInterestDto()
                        {
                            Id=6,
                            Name="",
                            Description=""
                        }
                    }
                }
            };
        }
    }
}
