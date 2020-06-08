using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationManagementApp.Models.EmployeeShiftModel
{
    public class EmployeeShiftModel
    {
        public EmployeesShifts EmployeeShifts { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<EmployeesShifts> EmployeesShifts { get; set; }


    }
}
