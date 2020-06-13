using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Employees
    {
        public Employees()
        {
            EmployeesShifts = new HashSet<EmployeesShifts>();
            Reservations = new HashSet<Reservations>();
            ServicesEmployees = new HashSet<ServicesEmployees>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Surname2 { get; set; }
        public string IdCard { get; set; }

        public virtual ICollection<EmployeesShifts> EmployeesShifts { get; set; }
        public virtual ICollection<Reservations> Reservations { get; set; }
        public virtual ICollection<ServicesEmployees> ServicesEmployees { get; set; }
    }
}
