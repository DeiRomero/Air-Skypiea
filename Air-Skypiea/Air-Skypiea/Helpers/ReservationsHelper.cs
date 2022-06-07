using Air_Skypiea.Common;
using Air_Skypiea.Data;
using Air_Skypiea.Data.Entities;
using Air_Skypiea.Models;

namespace Air_Skypiea.Helpers
{
    public class ReservationsHelper : IReservationsHelper
    {
        private readonly DataContext _context;

        public ReservationsHelper(DataContext context)
        {
            _context = context;

        }
        public async Task<Response> ProcessOrderAsync(ShowCartViewModel model)
        {
            Response response = await CheckInventoryAsync(model);
            if (!response.IsSuccess)
            {
                return response;
            }

            Sale sale = new()
            {
                Date = DateTime.UtcNow,
                User = model.User,
                Remarks = model.Remark,
                SaleDetails = new List<SaleDetail>(),
                FlightStatus=Enums.FlightStatus.Confirmado
            };

            foreach (Reservation? item in model.Reservations)
            {
                sale.SaleDetails.Add(new SaleDetail
                {
                    Flight = item.Flight,
                    Quantity = item.Quantity,
                    Remarks = item.Remark,
                });

                Flight flight = await _context.Flights.FindAsync(item.Flight.Id);
                if (flight != null)
                {
                   
                    _context.Flights.Update(flight);
                }

                _context.Reservations.Remove(item);
            }

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();
            return response;

        }
        private async Task<Response> CheckInventoryAsync(ShowCartViewModel model)
        {
            Response response = new() { IsSuccess = true };
            foreach (Reservation? item in model.Reservations)
            {
                Flight flight = await _context.Flights.FindAsync(item.Flight.Id);
                if (flight == null)
                {
                    response.IsSuccess = false;
                    response.Message = $"El producto {item.Flight.Date}, ya no está disponible";
                    return response;
                }
              
            }
            return response;
        }

    }
}
