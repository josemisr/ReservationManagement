using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class ServiceEmployeeDto
    {
        public int Id { get; set; }
        public int IdService { get; set; }
        public int IdEmployee { get; set; }

        public virtual EmployeeDto IdEmployeeNavigation { get; set; }
        public virtual ServiceDto IdServiceNavigation { get; set; }
    }
}
