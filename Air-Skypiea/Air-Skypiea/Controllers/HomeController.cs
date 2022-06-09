using Air_Skypiea.Common;
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
        private readonly IReservationsHelper _reservationsHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IMailHelper _mailHelper;
        private readonly ICombosHelper _combosHelper;

        public HomeController(ILogger<HomeController> logger, DataContext context,IUserHelper userHelper,IReservationsHelper reservationsHelper,IBlobHelper blobHelper,IMailHelper mailHelper,ICombosHelper combosHelper)
        {
            _logger = logger;
            _context = context;
            _userHelper = userHelper;
            _reservationsHelper = reservationsHelper;
            _blobHelper = blobHelper;
            _mailHelper = mailHelper;
            _combosHelper = combosHelper;
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

            HomeViewModel model = new() { Flights = flightsHome };
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user != null)
            {
                model.Quantity = await _context.Reservations
                    .Where(ts => ts.User.Id == user.Id)
                    .SumAsync(ts => ts.Quantity);
            }

            return View(model);


            
        
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

            Flight flight = await _context.Flights.FindAsync(id);
            if (flight == null)
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
                Flight = flight,
                Quantity = 1,
                User = user
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> ShowCart()
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }



            List<Reservation>? reservations = await _context.Reservations
            .Include(ts => ts.Flight)
            .ThenInclude(r=>r.Source)
            
             
            .Where(ts => ts.User.Id == user.Id)
            .ToListAsync();



            ShowCartViewModel model = new()
            {
                User = user,
               
                Reservations = reservations,
            };



            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowCart(ShowCartViewModel model)
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            model.User = user;
            model.Reservations = await _context.Reservations
                .Include(ts => ts.Flight)
             
                .Where(ts => ts.User.Id == user.Id)
                .ToListAsync();

            Response response = await _reservationsHelper.ProcessOrderAsync(model);
            if (response.IsSuccess)
            {
                return RedirectToAction(nameof(OrderSuccess));
            }

            ModelState.AddModelError(string.Empty, response.Message);
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Reservation reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            EditReservationViewModel model = new()
            {
                Id = reservation.id,
                Remark = reservation.Remark,

            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditReservationViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Reservation reservation = await _context.Reservations.FindAsync(id);
                   
                    reservation.Remark = model.Remark;
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                    return View(model);
                }

                return RedirectToAction(nameof(ShowCart));
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Reservation reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ShowCart));
        }


        [Authorize]
        public IActionResult OrderSuccess()
        {
            return View();
        }
         public async Task<IActionResult> Register()
        {
            AddUserViewModel model = new()
            {
                Id = Guid.Empty.ToString(),
                Countries = await _combosHelper.GetComboCountriesAsync(),
                States = await _combosHelper.GetComboStatesAsync(0),
                Cities = await _combosHelper.GetComboCitiesAsync(0),
                
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }

                model.ImageId = imageId;
                User user = await _userHelper.AddUserAsync(model);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Este correo ya está siendo usado.");
                    model.Countries = await _combosHelper.GetComboCountriesAsync();
                    model.States = await _combosHelper.GetComboStatesAsync(model.CountryId);
                    model.Cities = await _combosHelper.GetComboCitiesAsync(model.StateId);
                    return View(model);
                }

                string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                string tokenLink = Url.Action("ConfirmEmail", "Account", new
                {
                    userid = user.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendMail(
                    $"{model.FirstName} {model.LastName}",
                    model.Username,
                    "Air-Skypiea - Confirmación de Email",
                    $"<h1>Air-Skypiea - Confirmación de Email</h1>" +
                        $"Para habilitar el usuario por favor hacer click en el siguiente link:, " +
                        $"<hr/><br/><p><a href = \"{tokenLink}\">Confirmar Email</a></p>");
                if (response.IsSuccess)
                {
                    ViewBag.Message = "Las instrucciones para habilitar el usuario han sido enviadas al correo.";
                    return View(model);
                }

                ModelState.AddModelError(string.Empty, response.Message);

            }

            model.Countries = await _combosHelper.GetComboCountriesAsync();
            model.States = await _combosHelper.GetComboStatesAsync(model.CountryId);
            model.Cities = await _combosHelper.GetComboCitiesAsync(model.StateId);
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


    }
}