using Air_Skypiea.Data;
using Air_Skypiea.Enums;
using Air_Skypiea.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Air_Skypiea.Controllers
{
    
        [Authorize(Roles = "Admin")]
        public class DashboardController : Controller
        {
            private readonly DataContext _context;
            private readonly IUserHelper _userHelper;

            public DashboardController(DataContext context, IUserHelper userHelper)
            {
                _context = context;
                _userHelper = userHelper;
            }
            public async Task<IActionResult> Index()
            {
                ViewBag.UsersCount = _context.Users.Count();
                ViewBag.FlightsCount = _context.Flights.Count();
                ViewBag.NewOrdersCount = _context.Sales.Where(o => o.FlightStatus == FlightStatus.Confirmado).Count();
                ViewBag.ConfirmedOrdersCount = _context.Sales.Where(o => o.FlightStatus == FlightStatus.Cancelado).Count();

                return View(await _context.Reservations
                        .Include(u => u.User)
                        .Include(p => p.Flight).ToListAsync());
            }
        }

    }

