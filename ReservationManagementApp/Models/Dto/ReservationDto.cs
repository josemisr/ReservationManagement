using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationManagementApp.Models.Dto
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public int IdService { get; set; }
        public int IdEmployee { get; set; }
        public int IdUser { get; set; }
        public DateTime Date { get; set; }

        public virtual EmployeeDto IdEmployeeNavigation { get; set; }
        public virtual ServiceDto IdServiceNavigation { get; set; }
        public virtual UserDto IdUserNavigation { get; set; }
    }
}
