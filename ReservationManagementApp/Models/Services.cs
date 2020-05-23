using System;
using System.Collections.Generic;

namespace ReservationManagementApp.Models
{
    public partial class Services
    {
        public Services()
        {
            Reservations = new HashSet<Reservations>();
            ServicesEmployees = new HashSet<ServicesEmployees>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int DurationHours { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Reservations> Reservations { get; set; }
        public virtual ICollection<ServicesEmployees> ServicesEmployees { get; set; }
    }
}
