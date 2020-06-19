using Api.Models;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
