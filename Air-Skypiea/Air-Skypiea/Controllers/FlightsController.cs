using Air_Skypiea.Data;
using Air_Skypiea.Data.Entities;
using Air_Skypiea.Helpers;
using Air_Skypiea.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;
using static Air_Skypiea.Helpers.ModalHelper;

namespace Air_Skypiea.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FlightsController : Controller
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IFlashMessage _flashMessage;

        public FlightsController(DataContext context, ICombosHelper combosHelper, IBlobHelper blobHelper,IFlashMessage flashMessage)
        {
            _context = context;
            _combosHelper = combosHelper;
            _blobHelper = blobHelper;
            _flashMessage = flashMessage;
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
                    Price = model.Price,
                    Date = model.Date,
                };

 

                if (imageId != Guid.Empty)
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
       public async Task<IActionResult> Edit(int?id)
        {
            if(id==null)
            {
                return NotFound();
            }

            Flight flight = await _context.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }
            EditFlightViewModelcs model = new()
            {
                Id = flight.Id,

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
                Flight flight = await _context.Flights.FindAsync(id);
                _context.Update(flight);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un producto");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,dbUpdateException.InnerException.Message);
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
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
                .Include(p => p.FlightImages)
               
                .FirstOrDefaultAsync(p => p.Id == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }
        public async Task<IActionResult> AddImage(int? id)
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

            AddFlightImageViewModel model = new()
            {
                FlightId = flight.Id,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage(AddFlightImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;
                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "destinations");
                }

                Flight flight = await _context.Flights.FindAsync(model.FlightId);
                FlightImage flightImage = new()
                {
                    Flight = flight,
                    ImageId = imageId,
                };

                try
                {
                    _context.Add(flightImage);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { Id = flight.Id });
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return View(model);
        }
        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FlightImage flightImage = await _context.FlightImages
                .Include(pi => pi.Flight)
                .FirstOrDefaultAsync(pi => pi.Id == id);
            if (flightImage == null)
            {
                return NotFound();
            }

            await _blobHelper.DeleteBlobAsync(flightImage.ImageId, "destinations");
            _context.FlightImages.Remove(flightImage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { Id = flightImage.Flight.Id });
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Flight flight = await _context.Flights
                
                .Include(p => p.FlightImages)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Flight flight = await _context.Flights
                .Include(p => p.FlightImages)
                .FirstOrDefaultAsync(p => p.Id == id);

            foreach (FlightImage productImage in flight.FlightImages)
            {
                await _blobHelper.DeleteBlobAsync(productImage.ImageId, "destinations");
            }

            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



    }
}
