using Api.Models;
using System.Collections.Generic;

namespace Api.IServicesApi
{
    public interface IReservationService
    {
        List<ReservationDto> GetReservations();
        List<ReservationDto> GetAllAvailabilityReservations(int idEmployee, int idService, int idUser, string dateTime);
        ReservationDto GetReservationById(int id);
        ReservationDto AddReservation(ReservationDto reservationDto);
        ReservationDto UpdateReservation(ReservationDto reservationDto);
        ReservationDto RemoveReservation(int id);
    }
}
