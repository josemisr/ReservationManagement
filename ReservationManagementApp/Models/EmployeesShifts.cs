using System;
using System.Collections.Generic;
using System.Configuration;

namespace ReservationManagementApp.Models
{
    public partial class EmployeesShifts
    {
        public int Id { get; set; }
        public int IdEmployee { get; set; }
        public DateTime WorkDay { get; set; }
        [IntegerValidator(MinValue = 0, MaxValue =24)]
        public int InitHour { get; set; }
        [IntegerValidator(MinValue = 0, MaxValue = 24)]
        public int EndHour { get; set; }

        public virtual Employees IdEmployeeNavigation { get; set; }
    }
}
