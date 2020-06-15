﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
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
