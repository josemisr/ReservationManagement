using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationManagementApp.Models.Employee
{
    public class EmployeeModel
    {
        public Employees Employees { get; set; }
        public IEnumerable<ServicesEmployees> ServicesEmployees { get; set; }
        public IEnumerable<EmployeesShifts> EmployeesShifts { get; set; }
    }
}
