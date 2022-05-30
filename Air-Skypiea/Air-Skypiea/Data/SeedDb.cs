using Air_Skypiea.Data.Entities;
using Air_Skypiea.Enums;
using Air_Skypiea.Helpers;

namespace Air_Skypiea.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckFlightsAsync();
            await CheckCountriesAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Dei", "Romero", "Deirom@yopmail.com", "310 726 8748", "Calle Falsa Calle Perdida", UserType.Admin);
            await CheckUserAsync("1020", "Jen", "Sanchez", "Jensanchez@yopmail.com", "313 708 1677", "Calle Nose Calle Ahi", UserType.Admin);
            await CheckUserAsync("1030", "Santi", "Arias", "Santiarias@yopmail.com", "324 212 9806", "Calle Nose Calle Nose", UserType.Admin);
            await CheckUserAsync("1040", "Piccolo", "Daimao", "Picorodaimao@yopmail.com", "31O 897 8049", "Nameku", UserType.User);
        }

        private async Task CheckFlightsAsync()
        {
            if (!_context.Travels.Any())
            {
                _context.Travels.Add(new Travel
                {
                    Date = DateTime.Now,
                    Source = _context.Cities.FirstOrDefault(C => C.Id == 1),
                    Target = _context.Cities.FirstOrDefault(C => C.Id == 2),
                });
                await _context.SaveChangesAsync();
            }
        }

        private async Task<User> CheckUserAsync(
        string document,
        string firstName,
        string lastName,
        string email,
        string phone,
        string address,
        UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    City = _context.Cities.FirstOrDefault(),
                    UserType = userType,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }

            return user;
        }


        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());

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

            await _context.SaveChangesAsync();
        }

    }
}