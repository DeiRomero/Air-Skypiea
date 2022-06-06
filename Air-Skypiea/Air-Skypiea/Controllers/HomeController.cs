using Air_Skypiea.Data;
using Air_Skypiea.Data.Entities;
using Air_Skypiea.Helpers;
using Air_Skypiea.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Air_Skypiea.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public HomeController(ILogger<HomeController> logger, DataContext context, IUserHelper userHelper)
        {
            _logger = logger;
            _context = context;
            _userHelper = userHelper;
        }

        public async Task<IActionResult> Index()
        {
            List<Travel>? travels = await _context.Travels
                .ToListAsync();

            List<ProductsHomeViewModel> productsHome = new() { new ProductsHomeViewModel() };
            int i = 1;
            foreach (Travel? travel in travels)
            {
                if (i == 1)
                {
                    productsHome.LastOrDefault().Travel1 = travel;
                }
                if (i == 2)
                {
                    productsHome.LastOrDefault().Travel2 = travel;
                }
                if (i == 3)
                {
                    productsHome.LastOrDefault().Travel3 = travel;
                }
                if (i == 4)
                {
                    productsHome.LastOrDefault().Travel4 = travel;
                    productsHome.Add(new ProductsHomeViewModel());
                    i = 0;
                }
                i++;
            }

            HomeViewModel model = new() { Products = productsHome };
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user != null)
            {
                model.Quantity = await _context.Reservations
                    .Where(ts => ts.User.Id == user.Id)
                    .SumAsync(ts => ts.Quantity);
            }

            return View(model);
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

        public async Task<IActionResult> Add(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            Travel travel = await _context.Travels.FindAsync(id);
            if (travel == null)
            {
                return NotFound();
            }

            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            Reservation reservation = new()
            {
                Travel = travel,
                Quantity = 1,
                User = user
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //public async Task<IActionResult> ShowCart()
        //{

        //    return View();
        //}

        //[Authorize]
        //public async Task<IActionResult> ShowCart()
        //{
        //    User user = await _userHelper.GetUserAsync(User.Identity.Name);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    List<TemporalSale>? temporalSales = await _context.TemporalSales
        //        .Include(ts => ts.Product)
        //        .ThenInclude(p => p.ProductImages)
        //        .Where(ts => ts.User.Id == user.Id)
        //        .ToListAsync();

        //    ShowCartViewModel model = new()
        //    {
        //        User = user,
        //        TemporalSales = temporalSales,
        //    };

        //    return View(model);
        //}

    }
}