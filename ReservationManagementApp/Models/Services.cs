using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "Name Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Price Required")]
        public int Price { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Reservations> Reservations { get; set; }
        public virtual ICollection<ServicesEmployees> ServicesEmployees { get; set; }
    }
}
