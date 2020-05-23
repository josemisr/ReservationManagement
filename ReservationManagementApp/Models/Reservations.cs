using System;
using System.Collections.Generic;

namespace ReservationManagementApp.Models
{
    public partial class Reservations
    {
        public int Id { get; set; }
        public int IdService { get; set; }
        public int IdEmployee { get; set; }
        public int IdUser { get; set; }
        public DateTime Date { get; set; }

        public virtual Employees IdEmployeeNavigation { get; set; }
        public virtual Services IdServiceNavigation { get; set; }
        public virtual Users IdUserNavigation { get; set; }
    }
}
