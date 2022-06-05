using Air_Skypiea.Data;
using Air_Skypiea.Data.Entities;
using Air_Skypiea.Helpers;
using Air_Skypiea.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Air_Skypiea.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FlightsController : Controller
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IBlobHelper _blobHelper;

        public FlightsController(DataContext context, ICombosHelper combosHelper, IBlobHelper blobHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
            _blobHelper = blobHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Flights
                .Include(f => f.FlightImages)
                .Include(f => f.Source)
                .Include(f => f.Target)
                
                .ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            CreateFlightViewModel model = new()
            {
                Source = await _combosHelper.GetComboCitiesFAsync( ),
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
                if(model.ImageFile !=null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "destinations");
                }
                Flight flight = new()
                {
                    //Source=model.Source,
                    Price = model.Price,
                    Date = model.Date,
                };
                

               
                if(imageId != Guid.Empty)
                {
                    flight.FlightImages= new List<FlightImage>();
                    {
                        new FlightImage { ImageId = imageId };
                    }
                }
                try
                {
                    _context.Add(flight);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
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
