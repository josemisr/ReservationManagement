using System;
using System.Collections.Generic;

namespace ReservationManagementApp.Models
{
    public partial class Roles
    {
        public Roles()
        {
            Users = new HashSet<Users>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Descripción { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
