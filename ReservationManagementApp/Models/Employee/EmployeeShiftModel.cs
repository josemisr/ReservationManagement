using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationManagementApp.Models.EmployeeShiftModel
{
    public class EmployeeShiftModel
    {
        public EmployeesShifts EmployeeShift { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<EmployeesShifts> EmployeeShiftsList { get; set; }


    }
}
