using System.Collections.Generic;

namespace Api.Models
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Surname2 { get; set; }
        public string IdCard { get; set; }

        public ICollection<EmployeeShiftSimpleDto> EmployeesShifts { get; set; }
        public ICollection<ReservationSimpleDto> Reservations { get; set; }
        public ICollection<ServiceEmployeeSimpleDto> ServicesEmployees { get; set; }
    }
}
