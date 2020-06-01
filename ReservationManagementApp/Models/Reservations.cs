using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReservationManagementApp.Models
{
    public partial class Reservations
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Service Required")]
        public int IdService { get; set; }
        [Required(ErrorMessage = "Employee Required")]
        public int IdEmployee { get; set; }
        [Required(ErrorMessage = "User Required")]
        public int IdUser { get; set; }
        [Required(ErrorMessage = "Date Required")]
        public DateTime Date { get; set; }

        public virtual Employees IdEmployeeNavigation { get; set; }
        public virtual Services IdServiceNavigation { get; set; }
        public virtual Users IdUserNavigation { get; set; }
    }
}
