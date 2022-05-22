using Air_Skypiea.Data.Entities;

namespace Air_Skypiea.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCategoriesAsync();
            await CheckCountriesAsync();

        }
        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country
                {
                    Name = "Colombia",
                    
                    States = new List<State>()
                    {
                        new State{

                            Name = "Antioquia",                         

                            Cities = new List<City>()
                            {

                              new City { Name = "Medellín" },
                              new City { Name = "Rionegro" },

                            }
                        },

                        new State{

                            Name = "Bogota",

                            Cities = new List<City>()
                            {

                              new City { Name = "Fontibón" },                            

                            }
                        },

                         new State{

                            Name = "Cali",

                            Cities = new List<City>()
                            {

                              new City { Name = "Palmira" },
                           
                            }
                        },
                    }
                });

                _context.Countries.Add(new Country
                {
                    Name = "Venezuela",

                    States = new List<State>()
                    {
                        new State{

                            Name = "Carabobo",

                            Cities = new List<City>()
                            {

                              new City { Name = "Valencia" },
                           

                            }
                        },

                        new State{

                            Name = "Zulia",

                            Cities = new List<City>()
                            {

                              new City { Name = "Maracaibo" },

                            }
                        },

                         new State{

                            Name = "La Guiara",

                            Cities = new List<City>()
                            {

                              new City { Name = "Maiquetia" },

                            }
                        },

                    }
                });
            }

        }

        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "Nacionales" });
                _context.Categories.Add(new Category { Name = "Internacionales" });
                await _context.SaveChangesAsync();
            }
        }

    }
}