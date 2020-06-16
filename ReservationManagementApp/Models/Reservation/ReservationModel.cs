using ReservationManagementApp.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationManagementApp.Models.Reservation
{
    public class ReservationModel
    {
        public ReservationDto Reservation{ get; set; }
        public IEnumerable<ReservationDto> Reservations { get; set; }
    }
}
