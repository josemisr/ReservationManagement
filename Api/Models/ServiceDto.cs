using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class ServiceDto
    {
        public ServiceDto()
        {
            Reservations = new HashSet<ReservationDto>();
            ServicesEmployees = new HashSet<ServiceEmployeeDto>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ReservationDto> Reservations { get; set; }
        public virtual ICollection<ServiceEmployeeDto> ServicesEmployees { get; set; }
    }
}
