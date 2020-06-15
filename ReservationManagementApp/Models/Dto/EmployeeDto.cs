using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationManagementApp.Models.Dto
{
    public class EmployeeDto
    {
        public EmployeeDto()
        {
            EmployeesShifts = new HashSet<EmployeeShiftDto>();
            Reservations = new HashSet<ReservationDto>();
            ServicesEmployees = new HashSet<ServiceEmployeeDto>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Surname2 { get; set; }
        public string IdCard { get; set; }

        public virtual ICollection<EmployeeShiftDto> EmployeesShifts { get; set; }
        public virtual ICollection<ReservationDto> Reservations { get; set; }
        public virtual ICollection<ServiceEmployeeDto> ServicesEmployees { get; set; }
    }
}
