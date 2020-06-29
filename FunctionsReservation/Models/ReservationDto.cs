using System;

namespace FunctionsReservation.Models
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public int IdService { get; set; }
        public int IdEmployee { get; set; }
        public int IdUser { get; set; }
        public DateTime Date { get; set; }

        public EmployeeDto IdEmployeeNavigation { get; set; }
        public ServiceDto IdServiceNavigation { get; set; }
        public UserDto IdUserNavigation { get; set; }
    }
}
