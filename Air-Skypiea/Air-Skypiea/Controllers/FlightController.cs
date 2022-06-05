using Air_Skypiea.Data;
using Air_Skypiea.Data.Entities;
using Air_Skypiea.Helpers;
using Air_Skypiea.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;
using static Air_Skypiea.Helpers.ModalHelper;

namespace Air_Skypiea.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FlightController : Controller
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IFlashMessage _flashMessage;

        public FlightController(DataContext context, ICombosHelper combosHelper, IBlobHelper blobHelper, IFlashMessage flashMessage)
        {
            _context = context;
            _combosHelper = combosHelper;
            _blobHelper = blobHelper;
            _flashMessage = flashMessage;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Flights
              .Include(p => p.FlightImages)
              .Include(c => c.Source)
              .Include(c => c.Target)                     
              .ToListAsync());
         
        }

        public async Task<IActionResult> Create()
        {
            CreateFlightViewModel model = new()
            {
               
                Source = await _combosHelper.GetComboCitiesFAsync(),
                Target = await _combosHelper.GetComboCitiesFAsync(),
               
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFlightViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;
                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "flightimage");
                }


                Flight flight = new()
                {
                    Source = await _context.Cities.FindAsync(model.SourceId),
                    Target = await _context.Cities.FindAsync(model.TargetId),
                    Price = model.Price,
                    Date = model.Date,
                     

                };

                if (imageId != Guid.Empty)
                {
                    flight.FlightImages = new List<FlightImage>();
                    {
                        new FlightImage { ImageId = imageId };
                    }
                }

                try
                {
                    _context.Add(flight);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            
            model.Source = await _combosHelper.GetComboCitiesFAsync();
            model.Target = await _combosHelper.GetComboCitiesFAsync();
            return View(model);
        }


    }

}
