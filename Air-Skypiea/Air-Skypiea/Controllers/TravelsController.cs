using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Air_Skypiea.Data;
using Air_Skypiea.Data.Entities;
using Air_Skypiea.Models;
using Air_Skypiea.Helpers;
using Vereyon.Web;

namespace Air_Skypiea.Controllers
{
    public class TravelsController : Controller
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IBlobHelper _blobHelper;
        //private readonly IFlashMessage _flashMessage;

        public TravelsController(DataContext context, ICombosHelper combosHelper, IBlobHelper blobHelper/*, IFlashMessage flashMessage*/)
        {
            _context = context;
            _combosHelper = combosHelper;
            _blobHelper = blobHelper;
            //_flashMessage = flashMessage;
        }

        // GET: Travels
        public async Task<IActionResult> Index()
        {
              return View(await _context.Travels.ToListAsync());
        }

        // GET: Travels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Travels == null)
            {
                return NotFound();
            }

            var travel = await _context.Travels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (travel == null)
            {
                return NotFound();
            }

            return View(travel);
        }

        // GET: Travels/Create
        public async Task<IActionResult> Create()
        {
            CreateTravelViewModel model = new()
            {
                Source = await _combosHelper.GetComboTravelsAsync(),
                Target = await _combosHelper.GetComboTravelsAsync(),
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTravelViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Guid imageId = Guid.Empty;
                //if (model.ImageFile != null)
                //{
                //    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "destinations");
                //}
                Travel travel = new()
                {
                    Remark = model.Remark,
                    Date = model.Date,
                    //Source = await _context.Cities.FirstAsync(model.SourceId),
                    //Source = await _context.Cities.FirstAsync(model.SourceId)
                };

                //if (imageId != Guid.Empty)
                //{
                //    travel.FlightImages = new List<FlightImage>();
                //    {
                //        new FlightImage { ImageId = imageId };
                //    }
                //}
                try
                {
                    _context.Add(travel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            model.Source = await _combosHelper.GetComboTravelsAsync();
            model.Target = await _combosHelper.GetComboTravelsAsync();
            return View(model);
        }

        // GET: Travels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Travels == null)
            {
                return NotFound();
            }

            var travel = await _context.Travels.FindAsync(id);
            if (travel == null)
            {
                return NotFound();
            }
            return View(travel);
        }

        // POST: Travels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Remark")] Travel travel)
        {
            if (id != travel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(travel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TravelExists(travel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(travel);
        }

        // GET: Travels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Travels == null)
            {
                return NotFound();
            }

            var travel = await _context.Travels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (travel == null)
            {
                return NotFound();
            }

            return View(travel);
        }

        // POST: Travels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Travels == null)
            {
                return Problem("Entity set 'DataContext.Travels'  is null.");
            }
            var travel = await _context.Travels.FindAsync(id);
            if (travel != null)
            {
                _context.Travels.Remove(travel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TravelExists(int id)
        {
          return _context.Travels.Any(e => e.Id == id);
        }
    }
}
