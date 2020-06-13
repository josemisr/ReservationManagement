using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class EmployeesShifts
    {
        public int Id { get; set; }
        public int IdEmployee { get; set; }
        public DateTime WorkDay { get; set; }
        public int InitHour { get; set; }
        public int EndHour { get; set; }

        public virtual Employees IdEmployeeNavigation { get; set; }
    }
}
