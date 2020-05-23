using System;
using System.Collections.Generic;

namespace ReservationManagementApp.Models
{
    public partial class EmployeesShifts
    {
        public int Id { get; set; }
        public int IdEmployee { get; set; }
        public int Day { get; set; }
        public int InitHour { get; set; }
        public int EndHour { get; set; }

        public virtual Employees IdEmployeeNavigation { get; set; }
    }
}
