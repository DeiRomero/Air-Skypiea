using Air_Skypiea.Data.Entities;
using Air_Skypiea.Enums;
using Air_Skypiea.Helpers;

namespace Air_Skypiea.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;


        public SeedDb(DataContext context, IUserHelper userHelper,IBlobHelper blobHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();         
            await CheckCountriesAsync();
            await CheckRolesAsync();
            await CheckFlightsAsync();
            await CheckUserAsync("1010", "Dei", "Romero", "Deirom@yopmail.com", "310 726 8748", "Calle Falsa Calle Perdida", UserType.Admin);
            await CheckUserAsync("1020", "Jen", "Sanchez", "Jensanchez@yopmail.com", "313 708 1677", "Calle Nose Calle Ahi", UserType.Admin);
            await CheckUserAsync("1030", "Santi", "Arias", "Santiarias@yopmail.com", "324 212 9806", "Calle Nose Calle Nose", UserType.Admin);
            await CheckUserAsync("1040", "Piccolo", "Daimao", "Picorodaimao@yopmail.com", "31O 897 8049", "Nameku", UserType.User);
        }

        private async Task CheckFlightsAsync()
        {
            if (!_context.Flights.Any())

            {

                _context.Flights.Add(new Flight
                {

                    Source = _context.Cities.FirstOrDefault(c => c.Id == 1),
                    Target = _context.Cities.FirstOrDefault(c => c.Id == 2),
                    Price = 500000,
                    Date = new DateTime(2022, 6, 20),
                    FlightImages = new List<FlightImage>()

                    

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

                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);

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
                new State()
                {
                    Name = "Cundinamarca",
                    Cities = new List<City>() {
                        new City() { Name = "Usaquén" },
                        new City() { Name = "Guatavita" },
                             new City() { Name = "Parque Nacional Natural Chingaza" },

                    }
                },
                new State()
                {
                    Name = "Boyacá",
                    Cities = new List<City>() {
                        new City() { Name = "Villa De Leyva" },
                        new City() { Name = "Monguí" },
                         new City() { Name = "Parque Nacional Natura El Cocuy" },

                    }
                },
                new State()
                {
                    Name = "Huila",
                    Cities = new List<City>() {
                        new City() { Name = "Desierto De La Tatacoa" },

                    }
                },
                new State()
                {
                    Name = "Santander",
                    Cities = new List<City>() {

                        new City() { Name = "Barichara" },

                        new City() { Name = "San Gil" },

                    }
                },
                 new State()
                {
                    Name = "Antioquia",
                    Cities = new List<City>() {

                        new City() { Name = "Medellín" },

                    }
            }
                }
                });
                _context.Countries.Add(new Country
                {
                    Name = "Estados Unidos",
                    States = new List<State>()
            {
                new State()
                {
                    Name = "Florida",
                    Cities = new List<City>() {
                        new City() { Name = "Orlando" },

                    }

            }
                }
                });
                _context.Countries.Add(new Country
                {
                    Name = "China",
                    States = new List<State>()
            {
                new State()
                {
                    Name = "Shanhaiguan",
                    Cities = new List<City>() {
                        new City() { Name = "Golfo de Bohay" },
                    }
                },

            }
                });
                _context.Countries.Add(new Country
                {
                    Name = "Egipto",
                    States = new List<State>()
            {
                new State()
                {
                    Name = "El Cairo",
                    Cities = new List<City>() {
                        new City() { Name = "Necrópolis de Guiza" },
                    }
                },

            }
                });
                _context.Countries.Add(new Country
                {
                    Name = "Francia",
                    States = new List<State>()
            {
                new State()
                {
                    Name = "París",
                    Cities = new List<City>() {
                        new City() { Name = "París" },
                    }
                },

            }
                });
                _context.Countries.Add(new Country
                {
                    Name = "México",
                    States = new List<State>()
            {
                new State()
                {
                    Name = "Cancún y la Riviera Maya",
                    Cities = new List<City>() {
                        new City() { Name = "Playa del Carmen" },
                    }
                },

            }
                });


                await _context.SaveChangesAsync();
            }
        }
    }
}