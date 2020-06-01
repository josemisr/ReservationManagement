using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationManagementApp.Models.Reservation
{
    public class ReservationModel
    {
        public Reservations Reservation{ get; set; }
        public IEnumerable<Reservations> Reservations { get; set; }
    }
}
