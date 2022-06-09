using Air_Skypiea.Common;
using Air_Skypiea.Models;

namespace Air_Skypiea.Helpers
{
    public interface IReservationsHelper
    {
        Task<Response> ProcessOrderAsync(ShowCartViewModel model);
    }

}

