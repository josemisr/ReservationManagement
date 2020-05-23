using System;
using System.Collections.Generic;

namespace ReservationManagementApp.Models
{
    public partial class ServicesEmployees
    {
        public int Id { get; set; }
        public int IdService { get; set; }
        public int IdEmployee { get; set; }

        public virtual Employees IdEmployeeNavigation { get; set; }
        public virtual Services IdServiceNavigation { get; set; }
    }
}
