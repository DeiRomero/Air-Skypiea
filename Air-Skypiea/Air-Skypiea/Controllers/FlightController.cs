using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Air_Skypiea.Data;
using Air_Skypiea.Data.Entities;
using Air_Skypiea.Helpers;
using Air_Skypiea.Models;
using Vereyon.Web;
using System.Web.WebPages.Html;
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

        public async Task<IActionResult> Index()
        {
            return View(await _context.Flights
                .Include(p => p.FlightImages)
                .Include(p => p.Source)
                .Include(p => p.Target)
                .ToListAsync());
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
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "destinations");
                }

                Flight flight = new()
                {
                    Source = model.Source,
                    Target = model.Target,
                    Date = model.Date,
                    Price = model.Price,

                };



                if (imageId != Guid.Empty)
                {
                    flight.FlightImages = new List<FlightImage>()
                               {
                                   new FlightImage { ImageId = imageId }
                               };
                }

                try
                {
                    _context.Add(flight);
                    await _context.SaveChangesAsync();
                    _flashMessage.Confirmation("Registro creado.");
                    return Json(new
                    {
                        isValid = true,
                        html = ModalHelper.RenderRazorViewToString(this, "_ViewAllFlights", _context.Flights
                        .Include(p => p.FlightImages))


                    });

                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "Create", model) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(int id)
        {
            Flight flight = await _context.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }

            EditFlightViewModel model = new()
            {

                Id = flight.Id,
                Source = flight.Source,
                Target = flight.Target,
                Price = flight.Price,

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
                flight.Source = model.Source;
                flight.Target = model.Target;
                flight.Price = model.Price;
                _context.Update(flight);
                await _context.SaveChangesAsync();
                _flashMessage.Confirmation("Registro actualizado.");
                return Json(new
                {
                    isValid = true,
                    html = ModalHelper.RenderRazorViewToString(this, "_ViewAllFlights", _context.Flights
                    .Include(p => p.FlightImages))

                });
            }

            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "Edit", model) });
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


        [NoDirectAccess]
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
                Guid imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "destinations");
                Flight flight = await _context.Flights.FindAsync(model.FlightId);
                FlightImage FlightImage = new()
                {
                    Flight = flight,
                    ImageId = imageId,
                };

                try
                {
                    _context.Add(FlightImage);
                    await _context.SaveChangesAsync();
                    _flashMessage.Confirmation("Imagen agregada.");
                    return Json(new
                    {
                        isValid = true,
                        html = ModalHelper.RenderRazorViewToString(this, "Details", _context.Flights
                            .Include(p => p.FlightImages)

                            .FirstOrDefaultAsync(p => p.Id == model.FlightId))
                    });
                }
                catch (Exception exception)
                {
                    _flashMessage.Danger(exception.Message);
                }
            }

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddImage", model) });
        }

        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FlightImage FlightImage = await _context.FlightImages
                .Include(pi => pi.Flight)
                .FirstOrDefaultAsync(pi => pi.Id == id);
            if (FlightImage == null)
            {
                return NotFound();
            }

            await _blobHelper.DeleteBlobAsync(FlightImage.ImageId, "destinations");
            _context.FlightImages.Remove(FlightImage);
            await _context.SaveChangesAsync();
            _flashMessage.Info("Registro borrado.");
            return RedirectToAction(nameof(Details), new { id = FlightImage.Flight.Id });
        }


        [NoDirectAccess]
        public async Task<IActionResult> Delete(int id)
        {
            Flight flight = await _context.Flights
                .Include(p => p.FlightImages)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (flight == null)
            {
                return NotFound();
            }

            foreach (FlightImage FlightImage in flight.FlightImages)
            {
                await _blobHelper.DeleteBlobAsync(FlightImage.ImageId, "destinations");
            }

            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
            _flashMessage.Info("Registro borrado.");
            return RedirectToAction(nameof(Index));
        }

    }
}


        

    
