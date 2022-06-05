using Air_Skypiea.Data;
using Air_Skypiea.Data.Entities;
using Air_Skypiea.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Air_Skypiea.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _context;

        public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Flight>? flights = await _context.Flights
                .Include(f => f.Source)
                .Include(f => f.Target)
                .ToListAsync();


            List<FlightsHomeViewModel> flightsHome = new() { new FlightsHomeViewModel() };
            int i = 1;
            foreach (Flight? flight in flights)
            {
                if (i == 1)
                {
                    flightsHome.LastOrDefault().Flight1 = flight;
                }
                if (i == 2)
                {
                    flightsHome.LastOrDefault().Flight2 = flight;
                }
                if (i == 3)
                {
                    flightsHome.LastOrDefault().Flight3 = flight;
                }
                if (i == 4)
                {
                    flightsHome.LastOrDefault().Flight4 = flight;
                    flightsHome.Add(new FlightsHomeViewModel());
                    i = 0;
                }
                i++;
            }

            return View(flightsHome);
        
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("error/404")]
        public IActionResult Error404()
        {
            return View();
        }


    }
}