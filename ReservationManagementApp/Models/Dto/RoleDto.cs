using System.Collections.Generic;

namespace ReservationManagementApp.Models.Dto
{
    public class RoleDto
    {
        public RoleDto()
        {
            Users = new HashSet<UserDto>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Descripción { get; set; }

        public virtual ICollection<UserDto> Users { get; set; }
    }
}
