using System;
using System.Configuration;

namespace ReservationManagementApp.Models.Dto
{
    public class EmployeeShiftDto
    {
        public int Id { get; set; }
        public int IdEmployee { get; set; }
        public DateTime WorkDay { get; set; }
        [IntegerValidator(MinValue = 0, MaxValue = 24)]
        public int InitHour { get; set; }
        [IntegerValidator(MinValue = 0, MaxValue = 24)]
        public int EndHour { get; set; }

        public virtual EmployeeDto IdEmployeeNavigation { get; set; }
    }
}
