using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class UserDto
    {
        public UserDto()
        {
            Reservations = new HashSet<ReservationDto>();
        }

        public int Id { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Surname2 { get; set; }
        public string IdCard { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public int IdRole { get; set; }
        public string Password { get; set; }
        public virtual RoleDto IdRoleNavigation { get; set; }
        public virtual ICollection<ReservationDto> Reservations { get; set; }
    }
}
