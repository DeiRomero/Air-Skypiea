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
                Flight flight = new()
                {

                    Price = model.Price,
                    Date = model.Date,
                    Source= await _context.Cities.FindAsync(model.SourceId),
                    Target= await _context.Cities.FindAsync(model.TargetId),
                   


                };
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
                
               
                .FirstOrDefaultAsync(p => p.Id == id);
            if (flight == null)
            {
                return NotFound();
            }

            
            return View(flight);
        }

        public async Task<IActionResult> Delete(int id)
        {
            Flight flight = await _context.Flights

                .FirstOrDefaultAsync(p => p.Id == id);
            if (flight == null)
            {
                return NotFound();
            }


            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
            _flashMessage.Info("Registro borrado.");
            return RedirectToAction(nameof(Index));
        }


    }



    }

