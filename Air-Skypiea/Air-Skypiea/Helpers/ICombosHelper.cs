using Microsoft.AspNetCore.Mvc.Rendering;

namespace Air_Skypiea.Helpers
{
    public interface ICombosHelper
    {
        Task<IEnumerable<SelectListItem>> GetComboCountriesAsync();

        Task<IEnumerable<SelectListItem>> GetComboStatesAsync(int countryId);

        Task<IEnumerable<SelectListItem>> GetComboCitiesAsync(int stateId);

        Task<IEnumerable<SelectListItem>> GetComboCitiesFAsync();

    }
}
