using Air_Skypiea.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Air_Skypiea.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;
        }



       

        public async Task<IEnumerable<SelectListItem>> GetComboCitiesAsync(int stateId)
        {
            List<SelectListItem> list = await _context.Cities
                .Where(s => s.State.Id == stateId)
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
                .OrderBy(c => c.Text)
                .ToListAsync();

            list.Insert(0, new SelectListItem { Text = "[Selecciones una ciudad...]", Value = "0" });
            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboCitiesFAsync()
        {
            List<SelectListItem> list = await _context.Cities.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            })
              .OrderBy(c => c.Text)
              .ToListAsync();

            list.Insert(0, new SelectListItem { Text = "[Selecciones una ciudad...", Value = "0" });
            return list;

        }

        public async Task<IEnumerable<SelectListItem>> GetComboCountriesAsync()
        {
            List<SelectListItem> list = await _context.Countries.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            })
               .OrderBy(c => c.Text)
               .ToListAsync();

            list.Insert(0, new SelectListItem { Text = "[Selecciones un país...]", Value = "0" });
            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboStatesAsync(int countryId)
        {
            List<SelectListItem> list = await _context.States
                .Where(s => s.Country.Id == countryId)
                .Select(c => new SelectListItem
            {              
                Text = c.Name,
                Value = c.Id.ToString()
            })
                .OrderBy(c => c.Text)
                .ToListAsync();

            list.Insert(0, new SelectListItem { Text = "[Selecciones un Departamento / Estado...]", Value = "0" });
            return list;
        }

       
    }
}
