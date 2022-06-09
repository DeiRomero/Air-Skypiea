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

                Flight flight = new()
                {
                    Id = model.Id,                 
                    Source = await _context.Cities.FindAsync(model.SourceId),
                    Target = await _context.Cities.FindAsync(model.TargetId),
                    Price = model.Price,
                    Date = model.Date,

                };

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

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Flight flight = await _context.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }

            EditFlightViewModel model = new()
            {
                Id=flight.Id,
                Source =flight.Source,
                Target = flight.Target,
                Price = flight.Price,
                Date = flight.Date,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateFlightViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            try
            {
                Flight flight = await _context.Flights.FindAsync(model.Id);

                
                flight.Price = model.Price;
                flight.Date = model.Date;
                _context.Update(flight);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un producto con el mismo nombre.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }

            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Flight flight = await _context.Flights 
                
                .Include(pc => pc.Source)
                 .Include(pc => pc.Target)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }




    }

}
