using ReservationManagementApp.Models.Dto;
using System.Collections.Generic;

namespace ReservationManagementApp.Models.Reservation
{
    public class ReservationModel
    {
        public ReservationDto Reservation{ get; set; }
        public IEnumerable<ReservationDto> Reservations { get; set; }
    }
}
